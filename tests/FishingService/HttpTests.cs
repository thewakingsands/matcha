using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Cafe.Matcha.Utils;
using System.Threading.Tasks;
using Cafe.Matcha.Network.Fishing;
internal static class HttpTests
{
    private sealed class Handler : HttpMessageHandler
    {
        public HttpResponseMessage Response;
        public Action<HttpRequestMessage> Inspect;
        public bool WaitForCancellation;
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage r, CancellationToken c)
        {
            if (r.Headers.UserAgent.ToString() != "Cafe.Matcha/" + Cafe.Matcha.Data.Version)
                throw new Exception("missing shared user agent");
            Inspect?.Invoke(r);
            if (WaitForCancellation) await Task.Delay(Timeout.Infinite, c);
            return Response;
        }
    }
    internal static async Task Run()
    {
        var handler = new Handler { Response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"spot_id\":20,\"distributions\":[]}") } };
        using (var api = new FishingApi(handler))
        {
            if ((await api.Query(20, CancellationToken.None)).Length != 0) throw new Exception("empty query");
            var messages = new List<string>();
            Cafe.Matcha.Utils.Log.EventHandler onLog = (_, __, message) => messages.Add(message);
            Log.Handler += onLog;
            try
            {
                var payload = new byte[] { 0x80 }; // Synthetic empty CBOR array.
                handler.Response = new HttpResponseMessage(HttpStatusCode.NoContent);
                handler.Inspect = request =>
                {
                    if (request.Method != HttpMethod.Post || request.RequestUri.AbsolutePath != "/fisher/upload")
                        throw new Exception("upload route");
                    if (request.Content.Headers.ContentType.MediaType != "application/cbor" ||
                        request.Headers.GetValues("X-Client-Timestamp").Single() != "123")
                        throw new Exception("upload headers");
                    if (!request.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult().SequenceEqual(payload))
                        throw new Exception("upload content");
                };
                await api.Upload(payload, 123, CancellationToken.None);
#if DEBUG
                if (messages.Count != 2 || messages[0] != "POST https://api.ffxiv.cyou/fisher/upload" ||
                    messages[1] != "[Response] NoContent")
                    throw new Exception("upload must log request and status only");
#else
                if (messages.Count != 0) throw new Exception("release request logs must be disabled");
#endif
                messages.Clear();
                handler.Inspect = null;
                handler.Response = new HttpResponseMessage(HttpStatusCode.OK)
                { Content = new StringContent("{\"spot_id\":20,\"distributions\":[]}") };
                handler.Response.Headers.Add("X-Test-Header", new[] { "first", "second" });
                handler.Response.Headers.Add("Set-Cookie", "synthetic-session=private");
                await api.Query(20, CancellationToken.None);
#if DEBUG
                if (messages[0] != "GET https://api.ffxiv.cyou/fisher/fish-duration" ||
                    !messages.Contains("[Response] OK") ||
                    !messages.Contains("[Response Header] X-Test-Header: first, second") ||
                    !messages.Any(m => m.StartsWith("[Response Header] Content-Type: text/plain")) ||
                    !messages.Contains("[Response Header] Set-Cookie: synthetic-session=private"))
                    throw new Exception("query must log response and content headers");
                if (messages.Any(m => m.Contains("spot_id") || m.Contains("distributions")))
                    throw new Exception("query logs leaked body or parameters");
#else
                if (messages.Count != 0) throw new Exception("release header logs must be disabled");
#endif
                messages.Clear();
                handler.Inspect = _ => throw new HttpRequestException("synthetic-private-text");
                try { await api.Query(20, CancellationToken.None); throw new Exception("expected network failure"); }
                catch (HttpRequestException) { }
#if DEBUG
                if (messages.Count != 2 || messages[1] != "[Error] HttpRequestException")
                    throw new Exception("network error must be logged without exception content");
#endif
                if (messages.Any(m => m.Contains("synthetic-private-text")))
                    throw new Exception("private error text leaked");
            }
            finally { Log.Handler -= onLog; handler.Inspect = null; }

            handler.WaitForCancellation = true;
            using (var cancel = new CancellationTokenSource(TimeSpan.FromMilliseconds(30)))
            {
                try { await api.Query(20, cancel.Token); throw new Exception("expected cancellation"); }
                catch (OperationCanceledException) when (cancel.IsCancellationRequested) { }
            }
            handler.WaitForCancellation = false;

            handler.Response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"spot_id\":20,\"distributions\":[{}]}") };
            bool failed = false; try { await api.Query(20, CancellationToken.None); } catch { failed = true; }
            if (!failed) throw new Exception("missing fields must not become zero");
            handler.Response = new HttpResponseMessage((HttpStatusCode)429);
            handler.Response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromMinutes(3));
            try { await api.Query(20, CancellationToken.None); throw new Exception("expected rate limit"); }
            catch (FishingApiException e) { if (e.RetryAfter.TotalMinutes != 3) throw new Exception("retry-after"); }
        }
    }
}
