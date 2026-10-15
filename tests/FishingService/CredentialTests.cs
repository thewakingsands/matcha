using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cafe.Matcha.Network.Fishing;

internal static class CredentialTests
{
    private sealed class Handler : HttpMessageHandler
    {
        public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> Respond;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage r, CancellationToken t) => Respond(r, t);
    }
    private static void Check(bool value, string label) { if (!value) throw new Exception(label); }
    private static HttpResponseMessage Response(string cookie = null)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { spot_id = 20, distributions = new object[0] })) };
        if (cookie != null) response.Headers.Add("Set-Cookie", cookie);
        return response;
    }
    internal static async Task Run()
    {
        const string auth = "ffa_user_auth=synthetic-token; Expires=Sun, 12 Oct 2036 00:00:00 GMT; Path=/; Secure; HttpOnly";
        string saved = null;
        var credentials = new FishingCredentials(null, v => saved = v);
        credentials.SetEnabled(true);
        var handler = new Handler { Respond = (request, token) => {
            Check(!request.Headers.Contains("Cookie"), "first request has no auth");
            return Task.FromResult(Response(auth));
        }};
        using (var api = new FishingApi(handler, credentials))
        {
            await api.Upload(new byte[] { 0x80 }, 100, CancellationToken.None);
            Check(saved == auth, "persist cookie with expiry");
            handler.Respond = (request, token) => {
                Check(string.Join("", request.Headers.GetValues("Cookie")) == "ffa_user_auth=synthetic-token", "cookie attached without attributes");
                return Task.FromResult(Response("unrelated=ignored; Path=/"));
            };
            await api.Query(20, CancellationToken.None);
            Check(saved == auth, "unrelated cookie must not overwrite auth");
            await api.Upload(new byte[] { 0x80 }, 200, CancellationToken.None);
            var late = new TaskCompletionSource<HttpResponseMessage>();
            handler.Respond = (_, __) => late.Task;
            var pending = api.Upload(new byte[] { 0x80 }, 100, CancellationToken.None);
            credentials.SetEnabled(false);
            Check(saved == null, "revoke clears persisted credential");
            credentials.SetEnabled(true);
            late.SetResult(Response(auth));
            await pending;
            Check(saved == null, "late response cannot resurrect auth after reauthorization");
            using (var cancel = new CancellationTokenSource())
            {
                var delayed = new TaskCompletionSource<HttpResponseMessage>();
                handler.Respond = (_, __) => delayed.Task;
                var cancelled = api.Upload(new byte[] { 0x80 }, 100, cancel.Token);
                cancel.Cancel(); delayed.SetResult(Response(auth));
                try { await cancelled; } catch (OperationCanceledException) { }
                Check(saved == null, "cancelled request cannot store auth");
            }
        }

        // A new instance restores configuration, but never creates a request while disabled.
        credentials = new FishingCredentials(auth, v => saved = v);
        credentials.SetEnabled(true);
        handler = new Handler { Respond = (request, token) => {
            Check(string.Join("", request.Headers.GetValues("Cookie")) == "ffa_user_auth=synthetic-token", "restore saved auth");
            return Task.FromResult(Response("ffa_user_auth=; Expires=Sat, 01 Jan 2000 00:00:00 GMT; Path=/"));
        }};
        using (var api = new FishingApi(handler, credentials))
        {
            await api.Query(20, CancellationToken.None);
            Check(saved == null, "server expires cookie");
            credentials.SetEnabled(false);
            handler.Respond = (_, __) => throw new Exception("disabled request reached network");
            try { await api.Query(20, CancellationToken.None); throw new Exception("disabled request allowed"); }
            catch (OperationCanceledException) { }
        }
        foreach (var initial in new[] { "ffa_user_auth=old; Expires=Sat, 01 Jan 2000 00:00:00 GMT; Path=/", "invalid cookie" })
        {
            credentials = new FishingCredentials(initial, v => saved = v);
            credentials.SetEnabled(true);
            handler = new Handler { Respond = (r, t) => {
                Check(!r.Headers.Contains("Cookie"), "expired/malformed credential omitted");
                return Task.FromResult(Response());
            }};
            using (var api = new FishingApi(handler, credentials)) await api.Query(20, CancellationToken.None);
            Check(saved == null, "invalid persisted auth removed");
        }
    }
}
