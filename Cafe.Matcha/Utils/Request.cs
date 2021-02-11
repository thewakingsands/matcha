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

        public static async Task<HttpResponseMessage> Send(string endpoint, HttpMethod method, RequestHeaders header, HttpContent data)
        {
#if DEBUG
            Log.Debug($"[Request] {method} {endpoint}");
#endif
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

                    request.Headers.Add("User-Agent", $"Cafe.Matcha/{Data.Version}");
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
#if DEBUG
                        var body = await data.ReadAsStringAsync();
                        Log.Debug($"[Request-Content] {body}");
#endif
                    }

                    var res = await client.SendAsync(request);
#if DEBUG
                    var content = await res.Content.ReadAsStringAsync();
                    Log.Debug($"[Request-Response] {res.StatusCode} {content}");
#endif

                    return res;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Log.Debug($"[Request] {e.Message}");
#endif
                return null;
            }
        }
    }
}
