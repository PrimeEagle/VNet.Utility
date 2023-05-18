using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VNet.Utility
{
    public static class WebUtility
    {
        public static async Task<T> Get<T>(Uri uri, AuthenticationHeaderValue authHeader = null,
                   IDictionary<string, string> headers = null,
                   IEnumerable<MediaTypeWithQualityHeaderValue> acceptHeaders = null)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            if (authHeader is not null)
            {
                client.DefaultRequestHeaders.Authorization = authHeader;
            }

            if (acceptHeaders is not null)
            {
                foreach (var h in acceptHeaders)
                {
                    client.DefaultRequestHeaders.Accept.Add(h);
                }
            }

            if (headers is not null)
            {
                foreach (var (key, value) in headers)
                    client.DefaultRequestHeaders.Add(key, value);
            }

            var streamTask = client.GetStreamAsync(uri);

            var result = await JsonSerializer.DeserializeAsync<T>(await streamTask);

            return result;
        }

        public static async Task<HttpResponseMessage> Put<T>(Uri uri,
            T postObject,
            AuthenticationHeaderValue authHeader = null,
            IDictionary<string, string> headers = null,
            string mediaType = null)
        {
            var client = new HttpClient();

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(postObject, serializeOptions),
                                    Encoding.UTF8,
                                    mediaType)
            };

            request.Headers.Accept.Clear();
            if (authHeader is not null)
            {
                request.Headers.Authorization = authHeader;
            }

            if (headers is not null)
            {
                foreach (var (key, value) in headers)
                    request.Headers.Add(key, value);
            }

            var response = await client.SendAsync(request);

            return response;
        }
    }
}
