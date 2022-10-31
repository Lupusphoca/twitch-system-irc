namespace PierreARNAUDET.TwitchUtilitary.EventSub.Extensions
{
#nullable enable
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public static class HttpClientJsonExtensions
    {
        public static Task<HttpResponseMessage> PostAsNewtonsoftJsonAsync<TValue>(
            this HttpClient client, Uri? requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            var content = new StringContent(JsonConvert.SerializeObject(value, Formatting.None), Encoding.UTF8, "application/json");
            var contentString = JsonConvert.SerializeObject(value, Formatting.None);
            return client.PostAsync(requestUri, content, cancellationToken);
        }
    }
}