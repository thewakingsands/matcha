namespace Cafe.Matcha.Network.Fishing
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Cafe.Matcha.Utils;
    using Newtonsoft.Json;

    internal interface IFishingApi : IDisposable
    {
        Task<FishingDistribution[]> Query(uint place, CancellationToken cancellation);
        Task Upload(byte[] data, long timestamp, CancellationToken cancellation);
    }
    internal sealed class FishingApiException : Exception
    {
        public TimeSpan RetryAfter { get; }
        public int StatusCode { get; }
        public FishingApiException(TimeSpan retryAfter, int statusCode = 0) : base("Fishing service request failed")
        { StatusCode = statusCode; RetryAfter = retryAfter < TimeSpan.FromSeconds(60) ? TimeSpan.FromSeconds(60) : retryAfter; }
    }
    internal sealed class FishingApi : IFishingApi
    {
        private readonly HttpClient client;
        private readonly FishingCredentials credentials;
        public FishingApi(HttpMessageHandler handler = null, FishingCredentials credentials = null)
        {
            this.credentials = credentials ?? new FishingCredentials();
            if (credentials == null) this.credentials.SetEnabled(true);
            client = new HttpClient(handler ?? new HttpClientHandler { AllowAutoRedirect = false, UseCookies = false });
            client.BaseAddress = new Uri("https://api.ffxiv.cyou/fisher/");
            client.Timeout = TimeSpan.FromSeconds(5);
        }
        public async Task<FishingDistribution[]> Query(uint place, CancellationToken cancellation)
        {
            using (var deadline = CancellationTokenSource.CreateLinkedTokenSource(cancellation))
            {
                deadline.CancelAfter(TimeSpan.FromSeconds(5));
                return await QueryCore(place, deadline.Token).ConfigureAwait(false);
            }
        }
        private async Task<FishingDistribution[]> QueryCore(uint place, CancellationToken cancellation)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, "fish-duration?spot_id=" + place + "&min_count=10"))
            using (var response = await Send(request, cancellation).ConfigureAwait(false))
            {
                Check(response);
                if (response.Content.Headers.ContentLength > 2 * 1024 * 1024) throw new InvalidDataException("Fishing response too large");
                // Bound streaming reads too; Content-Length is optional and HttpClient's header timeout is insufficient.
                using (var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellation))
                using (var input = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (var output = new MemoryStream())
                {
                    timeout.CancelAfter(TimeSpan.FromSeconds(5));
                    var buffer = new byte[8192];
                    int read;
                    while ((read = await input.ReadAsync(buffer, 0, buffer.Length, timeout.Token).ConfigureAwait(false)) > 0)
                    {
                        if (output.Length + read > 2 * 1024 * 1024) throw new InvalidDataException("Fishing response too large");
                        output.Write(buffer, 0, read);
                    }
                    var result = JsonConvert.DeserializeObject<DurationResponse>(Encoding.UTF8.GetString(output.ToArray()));
                    if (result == null || result.SpotId != place || result.Distributions == null || result.Distributions.Length > 10000)
                        throw new InvalidDataException("Invalid fishing response");
                    foreach (var row in result.Distributions)
                        if (row == null || !row.Valid) throw new InvalidDataException("Invalid fishing distribution");
                    return result.Distributions;
                }
            }
        }
        public async Task Upload(byte[] data, long timestamp, CancellationToken cancellation)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, "upload"))
            {
                request.Content = new ByteArrayContent(data);
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/cbor");
                request.Headers.Add("X-Client-Timestamp", timestamp.ToString(System.Globalization.CultureInfo.InvariantCulture));
                using (var response = await Send(request, cancellation).ConfigureAwait(false)) Check(response);
            }
        }
        private async Task<HttpResponseMessage> Send(HttpRequestMessage request, CancellationToken cancellation)
        {
            var generation = credentials.Apply(request, cancellation);
            var response = await Request.Send(client, request, HttpCompletionOption.ResponseHeadersRead, cancellation).ConfigureAwait(false);
            try { credentials.Accept(response, generation, cancellation); return response; }
            catch { response.Dispose(); throw; }
        }
        private static void Check(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;
            var retry = response.Headers.RetryAfter;
            throw new FishingApiException(retry?.Delta ?? (retry?.Date - DateTimeOffset.UtcNow) ?? TimeSpan.FromSeconds(60), (int)response.StatusCode);
        }
        public void Dispose() { credentials.Stop(); client.Dispose(); }
    }
}
