namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.IO;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;
    using static PierreARNAUDET.TwitchUtilitary.TwitchPubSubEventStruct;

    //! To finalize this script of getting data from PubSub events, we need a real twitch channel capable of getting/receiving all this events
    class TwitchPubSubSystem : MonoBehaviour // https://github.com/paulbatum/WebSocket-Samples/blob/master/HttpListenerWebSocketEcho/Client/Client.cs
    {
        [field: SerializeField] public bool Verbose { get; private set; } = true;
        [field: SerializeField] public int SendChunkSize { get; private set; } = 64;
        [field: SerializeField] public int ReceiveChunkSize { get; private set; } = 32;
        
        [Data]
        [SerializeField] TwitchPubSubFilterBitsEvent filterBitsEvent;
        [SerializeField] TwitchPubSubFilterBitsBadge filterBitsBadge;
        [SerializeField] TwitchPubSubFilterChannelPoints filterChannelPoints;
        [SerializeField] TwitchPubSubFilterChannelSubscriptions filterChannelSubscriptions;
        [SerializeField] TwitchPubSubFilterAutoModQueue filterAutoModQueue;
        [SerializeField] TwitchPubSubFilterUserModerationNotification filterUserModerationNotification;


        private static object consoleLock = new object();
        private string[] topics;
        private string safetyCode;
        private bool pingPongValidation = false;

        private ClientWebSocket pubSubWebSocket;

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

        protected internal struct PubSubType
        {
            [JsonProperty("type", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }
        }

        private void OnEnable()
        {
            TwitchUtilitaryManager.OnAccessTokenAuthorized -= Connect;
            TwitchUtilitaryManager.OnAccessTokenAuthorized += Connect;
            safetyCode = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
        }

        private void OnApplicationQuit()
        {
            if (pubSubWebSocket != null)
            {
                pubSubWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application ended ...", CancellationToken.None);
            }
        }

        private void Connect()
        {
            _ = ConnectTask();
        }

        private async Task ConnectTask()
        {
            pubSubWebSocket = new ClientWebSocket();

            try
            {
                await pubSubWebSocket.ConnectAsync(new Uri("wss://pubsub-edge.twitch.tv"), CancellationToken.None);
                await Listen();
                await Task.WhenAll(Receive(), Send(), Ping());
            }
            catch (Exception ex)
            {
                Debug.Log($"Exception: {ex}");
            }
            finally
            {
                if (pubSubWebSocket != null)
                {
                    pubSubWebSocket.Dispose(); //* Release ressources from ClientWebSocket
                }
                lock (consoleLock)
                {
                    Debug.Log($"WebSocket closed.");
                }
            }
        }

        protected internal async Task Ping()
        {
            pingPongValidation = false;

            var timeSpan = TimeSpan.FromMinutes(4.5);
            await Task.Delay(timeSpan);

            var request = new PubSubType();
            request.Type = "PING";
            var json = JsonConvert.SerializeObject(request);

            var debug = $"{"PubSub Event".ColorString(ColorType.PubSubSend)} : PING Verification";
            Debug.Log(debug);
            twitchInformationBox.Display(debug);

            var encoded = Encoding.UTF8.GetBytes(json);
            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
            await pubSubWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            await Pong();
        }

        protected internal async Task Pong()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));

            if (pingPongValidation)
            {
                await Ping();
            }
            else
            {
                await Reconnect();
            }
        }

        protected internal async Task Reconnect()
        {
            await pubSubWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Reconnection process ...", CancellationToken.None);
            pubSubWebSocket.Dispose();
            await Task.Delay(TimeSpan.FromSeconds(5));
            Debug.Log("Restart PubSub connection".ColorString(ColorType.Error));
            Connect();
        }

        private async Task Listen()
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

            var debug = $"{"PubSub Event LISTEN Request JSON".ColorString(ColorType.PubSubSend)}";
            Debug.Log(debug);
            twitchInformationBox.Display(debug);

            var encoded = Encoding.UTF8.GetBytes(json);
            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
            await pubSubWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task Send()
        {
            // var random = new System.Random();
            // byte[] buffer = new byte[sendChunkSize];

            while (pubSubWebSocket.State == WebSocketState.Open)
            {
                // random.NextBytes(buffer);
                // await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, false, CancellationToken.None);
                // LogStatus("Send", buffer, buffer.Length);

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private async Task Receive()
        {
            while (pubSubWebSocket.State == WebSocketState.Open)
            {
                var buffer = new byte[ReceiveChunkSize];
                var totalBytesReceived = 0;
                var segment = new ArraySegment<byte>(buffer);
                WebSocketReceiveResult result = null;
                var memoryStream = new MemoryStream();

                do
                {
                    result = await pubSubWebSocket.ReceiveAsync(segment, CancellationToken.None);
                    memoryStream.Write(segment.Array, segment.Offset, result.Count);
                    totalBytesReceived += result.Count;
                } while (!result.EndOfMessage);

                memoryStream.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await pubSubWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }

                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    var reader = new StreamReader(memoryStream, Encoding.UTF8);
                    var data = reader.ReadToEnd();
                    AnalyseReceiveData(data);
                    LogStatus("Receive", buffer, data, totalBytesReceived);
                }
            }
        }

        //TODO : Resend analysed data into TwitchPubSubSystem (or a new MonoBehaviour script like TwitchPubSubOutEvent ?) that will send them into UnityEvent
        protected internal void AnalyseReceiveData(string data)
        {
            var parsedObject = JObject.Parse(data);
            var typeJson = parsedObject["type"].ToString();

            var debug = $"{"PubSub Receive Data ".ColorString(ColorType.CommonConsole)} : {typeJson} received";
            Debug.Log(debug);
            twitchInformationBox.Display(debug);

            switch (typeJson)
            {
                case "PONG":
                    pingPongValidation = true;
                    break;
                case "RECONNECT":
                    _ = Reconnect();
                    break;
                case "RESPONSE":
                    break;
                case "MESSAGE":
                    var topicJson = parsedObject["data"]["topic"].ToString();
                    if (topicJson.StartsWith("channel-bits-events-v1"))
                    {
                        //* Use Channel Bits Events V2
                    }
                    if (topicJson.StartsWith("channel-bits-events-v2"))
                    {
                        var bitsEventV2Message = JsonConvert.DeserializeObject<PubSubBitsEventV2Message>(parsedObject["message"].ToString());
                        filterBitsEvent.Filter(bitsEventV2Message);

                        debug = $"{"PubSub Event - Bits Event V2".ColorString(ColorType.Success)} - Received";
                        Debug.Log(debug);
                        twitchInformationBox.Display(debug);
                    }
                    if (topicJson.StartsWith("channel-bits-badge-unlocks"))
                    {
                        var bitsBadgeMessage = JsonConvert.DeserializeObject<PubSubBitsBadgeMessage>(parsedObject["message"].ToString());
                        filterBitsBadge.Filter(bitsBadgeMessage);

                        debug = $"{"PubSub Event - Bits Badge".ColorString(ColorType.Success)} - Received";
                        Debug.Log(debug);
                        twitchInformationBox.Display(debug);
                    }
                    if (topicJson.StartsWith("channel-subscribe-events-v1"))
                    {
                        var channelSubscriptionsMessage = JsonConvert.DeserializeObject<PubSubChannelSubscriptionsMessage>(parsedObject["message"].ToString());
                        filterChannelSubscriptions.Filter(channelSubscriptionsMessage);

                        debug = $"{"PubSub Event - Channel Subscriptions".ColorString(ColorType.Success)} - Received";
                        Debug.Log(debug);
                        twitchInformationBox.Display(debug);
                    }
                    if (topicJson.StartsWith("whispers"))
                    {
                        //* See IRC Chat Bot
                    }
                    if (topicJson.StartsWith("automod-queue"))
                    {
                        var autoModQueueMessage = JsonConvert.DeserializeObject<PubSubAutoModQueueMessage>(parsedObject["message"].ToString());
                        var autoModQueueContentClassification = JsonConvert.DeserializeObject<PubSubAutoModQueueContentClassification>(parsedObject["content_classification"].ToString());
                        var autoModQueueStatus = JsonConvert.DeserializeObject<PubSubAutoModQueueStatus>(parsedObject["status"].ToString());
                        filterAutoModQueue.FilterMessage(autoModQueueMessage);
                        filterAutoModQueue.FilterContentClassification(autoModQueueContentClassification);
                        filterAutoModQueue.FilterStatus(autoModQueueStatus);

                        debug = $"{"PubSub Event - Auto Mod Queue".ColorString(ColorType.Success)} - Received";
                        Debug.Log(debug);
                        twitchInformationBox.Display(debug);
                    }
                    if (topicJson.StartsWith("user-moderation-notifications"))
                    {
                        var userModerationNotificationMessage = JsonConvert.DeserializeObject<PubSubUserModerationNotificationMessage>(parsedObject["message"].ToString());
                        filterUserModerationNotification.Filter(userModerationNotificationMessage);

                        debug = $"{"PubSub Event - User Moderation Notification".ColorString(ColorType.Success)} - Received";
                        Debug.Log(debug);
                        twitchInformationBox.Display(debug);
                    }
                    break;
                case "reward-redeemed":
                    var channelPointsMessage = JsonConvert.DeserializeObject<PubSubChannelPointsMessage>(parsedObject["data"].ToString());
                    filterChannelPoints.Filter(channelPointsMessage);

                    debug = $"{"PubSub Event - Channel Points".ColorString(ColorType.Success)} - Received";
                    Debug.Log(debug);
                    twitchInformationBox.Display(debug);
                    break;
                default:
                    break;
            }
        }

        private void LogStatus(string status, byte[] buffer, string data, int bytesLength)
        {
            lock (consoleLock)
            {
                var debug = $"{$"PubSub Event {status} {bytesLength} bytes".ColorString(ColorType.PubSubReceive)} : {data}";
                Debug.Log(debug);
                twitchInformationBox.Display(debug);

                if (Verbose)
                {
                    Debug.Log(BitConverter.ToString(buffer, 0, bytesLength));
                }
            }
        }
    }
}