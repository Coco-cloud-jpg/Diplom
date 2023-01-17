﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Services
{
    internal class HttpClientService
    {
        private static readonly HttpClient _httpClient;
        static HttpClientService()
        {
            _httpClient = new HttpClient();
        }
        public async Task PostStreamAsync(string url, object content, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            using (var httpContent = CreateHttpContent(content))
            {
                request.Content = httpContent;

                using (var response = await _httpClient
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        public void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            {
                using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
                {
                    var js = new JsonSerializer();
                    js.Serialize(jtw, value);
                    jtw.Flush();
                }
            }
        }
        private HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
    }
}
