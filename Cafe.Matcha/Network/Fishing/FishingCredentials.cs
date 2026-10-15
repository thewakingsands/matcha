namespace Cafe.Matcha.Network.Fishing
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    internal sealed class FishingCredentials
    {
        private static readonly Uri Origin = new Uri("https://api.ffxiv.cyou/fisher/");
        private const string Name = "ffa_user_auth";
        private readonly object sync = new object();
        private readonly Action<string> save;
        private string stored;
        private bool enabled;
        private long generation;

        // Persist the single Set-Cookie value, including expiry, rather than unrelated cookies.
        public FishingCredentials(string stored = null, Action<string> save = null)
        { this.stored = stored; this.save = save ?? (_ => { }); }

        public void SetEnabled(bool value)
        {
            lock (sync)
            {
                if (enabled != value) generation++;
                enabled = value;
                if (!value) { stored = null; save(null); }
            }
        }
        public void Stop()
        {
            lock (sync) { enabled = false; generation++; }
        }
        public long Apply(HttpRequestMessage request, CancellationToken cancellation)
        {
            lock (sync)
            {
                cancellation.ThrowIfCancellationRequested();
                if (!enabled) throw new OperationCanceledException();
                if (!string.IsNullOrEmpty(stored))
                {
                    var cookie = Parse(stored);
                    if (cookie == null || cookie.Expired || string.IsNullOrEmpty(cookie.Value))
                    { stored = null; save(null); }
                    else request.Headers.Add("Cookie", Name + "=" + cookie.Value);
                }
                return generation;
            }
        }
        public void Accept(HttpResponseMessage response, long requestGeneration, CancellationToken cancellation)
        {
            lock (sync)
            {
                if (!enabled || requestGeneration != generation || cancellation.IsCancellationRequested) return;
                if (!response.Headers.TryGetValues("Set-Cookie", out var headers)) return;
                foreach (var header in headers)
                {
                    var cookie = Parse(header);
                    if (cookie == null) continue;
                    var value = cookie.Expired || string.IsNullOrEmpty(cookie.Value) ? null : header;
                    stored = value;
                    save(value);
                }
            }
        }
        private static Cookie Parse(string header)
        {
            if (header == null || header.Length > 8192 ||
                !header.TrimStart().StartsWith(Name + "=", StringComparison.Ordinal)) return null;
            try
            {
                var container = new CookieContainer();
                container.SetCookies(Origin, header);
                // Expired/deleted cookies are omitted by CookieContainer.
                var cookie = container.GetCookies(Origin)[Name];
                return cookie ?? new Cookie(Name, "") { Expired = true };
            }
            catch (CookieException) { return null; }
        }
    }
}
