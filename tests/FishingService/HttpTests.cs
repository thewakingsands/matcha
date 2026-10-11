using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cafe.Matcha.Network.Fishing;
internal static class HttpTests
{
    private sealed class Handler : HttpMessageHandler
    {
        public HttpResponseMessage Response;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage r, CancellationToken c) => Task.FromResult(Response);
    }
    internal static async Task Run()
    {
        var handler = new Handler { Response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"spot_id\":20,\"distributions\":[]}") } };
        using (var api = new FishingApi(handler))
        {
            if ((await api.Query(20, CancellationToken.None)).Length != 0) throw new Exception("empty query");
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
