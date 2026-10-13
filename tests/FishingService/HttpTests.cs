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
                if (messages.Count != 0) throw new Exception("transport must not log fishing data");
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
