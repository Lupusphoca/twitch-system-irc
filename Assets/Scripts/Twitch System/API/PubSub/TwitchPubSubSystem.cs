namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.Net.WebSockets;
    using Newtonsoft.Json;

    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;

    class TwitchPubSubSystem : MonoBehaviour // https://github.com/paulbatum/WebSocket-Samples/blob/master/HttpListenerWebSocketEcho/Client/Client.cs
    {
        #region Data
        [Settings]
        [field: SerializeField] protected internal bool Verbose { get; private set; } = true;
        [field: SerializeField] protected internal int SendChunkSize { get; private set; } = 64;
        [field: SerializeField] protected internal int ReceiveChunkSize { get; private set; } = 32;

        protected internal object consoleLock = new object();
        protected internal string[] topics;
        protected internal string safetyCode;
        protected internal bool pingPongValidation;

        protected internal struct PubSubListenRequest
        {
            [JsonProperty("type", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }
            [JsonProperty("nonce", Order = 2, NullValueHandling = NullValueHandling.Ignore)]
            public string Nonce { get; set; }
            [JsonProperty("data", Order = 3, NullValueHandling = NullValueHandling.Ignore)]
            public Data _Data;

            public struct Data
            {
                [JsonProperty("topics", Order = 4, NullValueHandling = NullValueHandling.Ignore)]
                public string[] Topics { get; set; }
                [JsonProperty("auth_token", Order = 5, NullValueHandling = NullValueHandling.Ignore)]
                public string AuthToken { get; set; }
            }
        }

        protected internal struct PubSubType
        {
            [JsonProperty("type", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }
        }
        #endregion

        private void OnEnable()
        {
            TwitchUtilitaryManager.OnAccessTokenAuthorized -= Connect;
            TwitchUtilitaryManager.OnAccessTokenAuthorized += Connect;
            safetyCode = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
        }

        protected internal void Connect()
        {
            pubSubWebSocket = new ClientWebSocket();
            var tpsConnection = new TwitchPubSubConnection(this);
            _ = tpsConnection.ConnectTask();
        }

        protected internal void LogStatus(string status, byte[] buffer, string data, int bytesLength)
        {
            lock (consoleLock)
            {
                Debug.Log($"{$"PubSub Event {status}".ColorString(ColorType.CommonConsole)} {bytesLength} bytes {data}");

                if (Verbose)
                {
                    Debug.Log(BitConverter.ToString(buffer, 0, bytesLength));
                }
            }
        }
    }
}