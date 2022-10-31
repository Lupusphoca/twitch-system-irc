namespace PierreARNAUDET.TwitchUtilitary.Helpers
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    using UnityEngine;
    
    public static class ParseHttpReponse
    {
        private static int retryCounter;

        public static async Task<T> ParseResponse<T>(HttpResponseMessage response, CancellationTokenSource cancellationToken, int maxRetries)
        {
            if (response?.RequestMessage == null)
            {
                return default;
            }

            if (response.RequestMessage.Content != null)
            {
                var requestContent = await response.RequestMessage.Content.ReadAsStringAsync();
                Debug.Log($"[{response.RequestMessage.Method}] -> {response.RequestMessage.RequestUri}: {requestContent.Trim()}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Debug.Log($"Failed Response [{response.RequestMessage.Method}] -> {response.RequestMessage.RequestUri} -> {responseContent.Trim()}");

                retryCounter++;
                if (retryCounter >= maxRetries)
                {
                    cancellationToken.Cancel(true);
                    return default;
                }
                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken.Token);

                return default;
            }

            retryCounter = 0;
            cancellationToken.Cancel(true);
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}