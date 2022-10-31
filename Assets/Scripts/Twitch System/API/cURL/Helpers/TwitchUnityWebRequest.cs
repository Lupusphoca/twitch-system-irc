namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;
    using UnityEngine.Networking;

    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;
    
    class TwitchUnityWebRequest
    {
        public async Task<string> Post(string url, Dictionary<string, string> formFields)
        {
            var webRequest = UnityWebRequest.Post(url, formFields);
            webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            webRequest.SendWebRequest();

            var result = string.Empty;

            while (!webRequest.isDone)
            {
                await Task.Yield();
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError($"<color={HexColorError}>Connection Error</color> {webRequest.error}");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"<color={HexColorError}>Data Processing Error</color> {webRequest.error}");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"<color={HexColorError}>Protocol Error</color> {webRequest.error}");
                    break;
                case UnityWebRequest.Result.Success:
                    result = webRequest.downloadHandler.text;
                    break;
            }

            return result;
        }

        public async Task<string> Get(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            webRequest.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            webRequest.SetRequestHeader("Client-Id", twitchCredentials.ClientId);
            webRequest.SendWebRequest();

            var result = string.Empty;

            while (!webRequest.isDone)
            {
                await Task.Yield();
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError($"<color={HexColorError}>Connection Error</color> {webRequest.error}");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"<color={HexColorError}>Data Processing Error</color> {webRequest.error}");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"<color={HexColorError}>Protocol Error</color> {webRequest.error}");
                    break;
                case UnityWebRequest.Result.Success:
                    result = webRequest.downloadHandler.text;
                    break;
            }

            return result;
        }
    }
}