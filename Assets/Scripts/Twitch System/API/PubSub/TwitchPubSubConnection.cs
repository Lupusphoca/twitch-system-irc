namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.IO;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;

    public class TwitchPubSubConnection : MonoBehaviour // https://github.com/paulbatum/WebSocket-Samples/blob/master/HttpListenerWebSocketEcho/Client/Client.cs
    {
        [Settings]
        [field: SerializeField] public bool Verbose { get; private set; } = true;
        [field: SerializeField] public int SendChunkSize { get; private set; } = 64;
        [field: SerializeField] public int ReceiveChunkSize { get; private set; } = 32;

        private static object consoleLock = new object();
        private static readonly TimeSpan delay = TimeSpan.FromMilliseconds(1000);
        private string[] topics;
        private string safetyCode;

        public struct PubSubListenRequest
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

        private void OnEnable()
        {
            TwitchUtilitaryManager.OnAccessTokenAuthorized -= Connect;
            TwitchUtilitaryManager.OnAccessTokenAuthorized += Connect;
            safetyCode = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
        }

        private void Connect()
        {
            _ = ConnectTask();
        }

        private async Task ConnectTask()
        {
            var webSocket = new ClientWebSocket();

            try
            {
                await webSocket.ConnectAsync(new Uri("wss://pubsub-edge.twitch.tv"), CancellationToken.None);
                await Listen(webSocket);
                await Task.WhenAll(Receive(webSocket), Send(webSocket));
            }
            catch (Exception ex)
            {
                Debug.Log($"Exception: {ex}");
            }
            finally
            {
                if (webSocket != null)
                {
                    webSocket.Dispose(); //* Release ressources from ClientWebSocket
                }
                lock (consoleLock)
                {
                    Debug.Log($"WebSocket closed.");
                }
            }
        }

        private async Task Listen(ClientWebSocket webSocket)
        {
            var request = new PubSubListenRequest();
            request.Type = "LISTEN";
            request.Nonce = safetyCode;
            topics = new string[] {
                $"channel-bits-events-v2.{twitchUser.ListData[0].Id}", //* VALIDATE
                $"channel-bits-badge-unlocks.{twitchUser.ListData[0].Id}", //* VALIDATE
                $"channel-points-channel-v1.{twitchUser.ListData[0].Id}",
                $"channel-subscribe-events-v1.{twitchUser.ListData[0].Id}", //* VALIDATE
                $"chat_moderator_actions.{twitchUser.ListData[0].Id}.{twitchUser.ListData[0].Id}", //* VALIDATE
                $"automod-queue.{twitchUser.ListData[0].Id}.{twitchUser.ListData[0].Id}", //* VALIDATE
                $"user-moderation-notifications.{twitchUser.ListData[0].Id}.{twitchUser.ListData[0].Id}", //* VALIDATE
                $"whispers.{twitchUser.ListData[0].Id}" //* VALIDATE
                };
            request._Data.Topics = topics;
            request._Data.AuthToken = accessToken;

            var json = JsonConvert.SerializeObject(request);
            Debug.Log($"{"PubSub Event LISTEN Request JSON".ColorString(ColorType.PubSubSend)} {json}");

            var encoded = Encoding.UTF8.GetBytes(json);
            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task Send(ClientWebSocket webSocket)
        {
            // var random = new System.Random();
            // byte[] buffer = new byte[sendChunkSize];

            while (webSocket.State == WebSocketState.Open)
            {
                // random.NextBytes(buffer);
                // await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, false, CancellationToken.None);
                // LogStatus("Send", buffer, buffer.Length);

                await Task.Delay(delay);
            }
        }

        private async Task Receive(ClientWebSocket webSocket)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var buffer = new byte[ReceiveChunkSize];
                var totalBytesReceived = 0;
                var segment = new ArraySegment<byte>(buffer);
                WebSocketReceiveResult result = null;
                var memoryStream = new MemoryStream();

                do
                {
                    result = await webSocket.ReceiveAsync(segment, CancellationToken.None);
                    memoryStream.Write(segment.Array, segment.Offset, result.Count);
                    totalBytesReceived += result.Count;
                } while (!result.EndOfMessage);

                memoryStream.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }

                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    var reader = new StreamReader(memoryStream, Encoding.UTF8);
                    LogStatus("Receive", buffer, reader.ReadToEnd(), totalBytesReceived);
                }
            }
        }

        private void LogStatus(string status, byte[] buffer, string data, int bytesLength)
        {
            lock (consoleLock)
            {
                Debug.Log($"{$"PubSub Event {status} {bytesLength}".ColorString(ColorType.PubSubReceive)} bytes {data}");

                if (Verbose)
                {
                    Debug.Log(BitConverter.ToString(buffer, 0, bytesLength));
                }
            }
        }
    }
}