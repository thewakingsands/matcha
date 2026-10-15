// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

/// <copyright>
///
/// </copyright>
namespace Cafe.Matcha.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Cafe.Matcha.Constant;
    using Newtonsoft.Json;

    internal class RequestHeaders : List<KeyValuePair<string, string>> { }

    internal class Request
    {
        private static RequestHeaders ParseHeader(string header)
        {
            if (string.IsNullOrEmpty(header))
            {
                return null;
            }

            var headers = new RequestHeaders();
            foreach (string line in header.Split('\n'))
            {
                var pos = line.IndexOf(':');
                if (pos == -1)
                {
                    continue;
                }

                headers.Add(new KeyValuePair<string, string>(line.Substring(0, pos).Trim(), line.Substring(pos + 1).Trim()));
            }

            return headers;
        }

        public static async Task<HttpResponseMessage> SendJson(string endpoint, string header, string json)
        {
            return await SendJson(endpoint, ParseHeader(header), json);
        }

        public static async Task<HttpResponseMessage> SendJson(string endpoint, RequestHeaders header, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await Send(endpoint, HttpMethod.Post, header, content);
        }

        public static async Task<HttpResponseMessage> SendAsJson(string endpoint, string header, object data)
        {
            return await SendAsJson(endpoint, ParseHeader(header), data);
        }

        public static async Task<HttpResponseMessage> SendAsJson(string endpoint, RequestHeaders header, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            return await SendJson(endpoint, header, json);
        }

        public static async Task<HttpResponseMessage> SendAsMultipart(string endpoint, RequestHeaders header, MultipartFormDataContent data)
        {
            return await Send(endpoint, HttpMethod.Post, header, data);
        }

        public static async Task<HttpResponseMessage> Send(string endpoint)
        {
            return await Send(endpoint, HttpMethod.Get, null, null);
        }

        public static async Task<HttpResponseMessage> Send(string endpoint, RequestType type, Dictionary<string, string> data)
        {
            return await Send(endpoint, type, (RequestHeaders)null, data);
        }

        public static async Task<HttpResponseMessage> Send(string endpoint, RequestType type, string header, Dictionary<string, string> data)
        {
            return await Send(endpoint, type, ParseHeader(header), data);
        }

        public static async Task<HttpResponseMessage> Send(string endpoint, RequestType type, RequestHeaders header, Dictionary<string, string> data)
        {
            switch (type)
            {
                case RequestType.Get:
                    StringBuilder sb = new StringBuilder(endpoint);

                    bool hasQuery = endpoint.Contains("?");
                    foreach (var pair in data)
                    {
                        sb.Append(hasQuery ? '&' : '?');
                        sb.Append(Uri.EscapeDataString(pair.Key));
                        sb.Append('=');
                        sb.Append(Uri.EscapeDataString(pair.Value));

                        hasQuery = true;
                    }

                    return await Send(sb.ToString(), HttpMethod.Get, header, null);
                case RequestType.JSON:
                    return await SendAsJson(endpoint, header, data);
                case RequestType.Form:
                    return await Send(endpoint, HttpMethod.Post, header, new FormUrlEncodedContent(data));
                case RequestType.Multipart:
                    MultipartFormDataContent multipartData = new MultipartFormDataContent();
                    foreach (var pair in data)
                    {
                        multipartData.Add(new StringContent(pair.Value), pair.Key);
                    }

                    return await Send(endpoint, HttpMethod.Post, header, multipartData);
            }

            return null;
        }

        // The caller owns the client, request and response. Exceptions and
        // cancellation propagate to callers that implement retries or consent checks.
        // Streaming/sensitive requests log metadata only; logging bodies buffers them.
        public static async Task<HttpResponseMessage> Send(
            HttpClient client,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            CancellationToken cancellationToken,
            bool logBodies = false)
        {
            try
            {
                request.Headers.UserAgent.Clear();
                request.Headers.UserAgent.ParseAdd($"Cafe.Matcha/{Data.Version}");
#if DEBUG
                var uri = request.RequestUri.IsAbsoluteUri
                    ? request.RequestUri : new Uri(client.BaseAddress, request.RequestUri);
                Log.Debug(LogType.Request, $"{request.Method} {(logBodies ? uri.ToString() : uri.GetLeftPart(UriPartial.Path))}");
                if (logBodies && request.Content != null)
                {
                    var body = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Log.Debug(LogType.Request, $"[Content] {body}");
                }
#endif
                var response = await client.SendAsync(request, completionOption, cancellationToken).ConfigureAwait(false);
#if DEBUG
                LogResponseHeaders(response.Headers);
                if (response.Content != null)
                {
                    LogResponseHeaders(response.Content.Headers);
                }
                if (logBodies && response.Content != null)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Log.Debug(LogType.Request, $"[Response] {response.StatusCode} {content}");
                }
                else
                {
                    Log.Debug(LogType.Request, $"[Response] {response.StatusCode}");
                }
#endif
                return response;
            }
            catch (Exception e)
            {
                LogFailure(e);
                throw;
            }
        }

#if DEBUG
        private static void LogResponseHeaders(System.Net.Http.Headers.HttpHeaders headers)
        {
            foreach (var header in headers)
            {
                var value = string.Join(", ", header.Value);
                Log.Debug(LogType.Request, $"[Response Header] {header.Key}: {value.Replace("\r", "\\r").Replace("\n", "\\n")}");
            }
        }
#endif

        [System.Diagnostics.Conditional("DEBUG")]
        private static void LogFailure(Exception error)
        {
#if DEBUG
            // Exception messages can contain URLs, credentials or remote content.
            Log.Error(LogType.Request, $"[Error] {error.GetType().Name}");
#endif
        }

        public static async Task<HttpResponseMessage> Send(string endpoint, HttpMethod method, RequestHeaders header, HttpContent data)
        {
            try
            {
                var uri = new Uri(endpoint);
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = uri,
                        Method = method
                    };

                    if (header != null)
                    {
                        foreach (var pair in header)
                        {
                            request.Headers.Add(pair.Key, pair.Value);
                        }
                    }

                    if (data != null)
                    {
                        request.Content = data;
                    }

                    return await Send(client, request, HttpCompletionOption.ResponseContentRead, CancellationToken.None, logBodies: true);
                }
            }
            catch (Exception)
            {
                // Preserve the legacy convenience API's null-on-failure behavior.
                return null;
            }
        }
    }
}
