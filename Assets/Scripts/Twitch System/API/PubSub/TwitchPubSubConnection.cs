namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    using UnityEngine;

    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;

    class TwitchPubSubConnection // https://github.com/paulbatum/WebSocket-Samples/blob/master/HttpListenerWebSocketEcho/Client/Client.cs
    {
        private TwitchPubSubSystem tpsSystem;
        private TwitchPubSubReceive tpsReceive;
        private TwitchPubSubSend tpsSend;
        private TwitchPubSubPingPong tpsPingPong;

        protected internal TwitchPubSubConnection(TwitchPubSubSystem tpsSystem)
        {
            this.tpsSystem = tpsSystem;
        }

        protected internal async Task ConnectTask()
        {
            try
            {
                await pubSubWebSocket.ConnectAsync(new Uri("wss://pubsub-edge.twitch.tv"), CancellationToken.None);
                await Listen();
                var tpsReceive = new TwitchPubSubReceive(tpsSystem);
                var tpsSend = new TwitchPubSubSend(tpsSystem);
                var tpsPingPong = new TwitchPubSubPingPong(tpsSystem);
                await Task.WhenAll(tpsReceive.Receive(), tpsSend.Send(), tpsPingPong.Ping());
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
                lock (tpsSystem.consoleLock)
                {
                    Debug.Log($"WebSocket closed.");
                }
            }
        }

        protected async Task Listen()
        {
            var request = new TwitchPubSubSystem.PubSubListenRequest();
            request.Type = "LISTEN";
            request.Nonce = tpsSystem.safetyCode;
            tpsSystem.topics = new string[] {
                $"channel-bits-events-v2.{mainTwitchUser.ListData[0].Id}",
                $"channel-bits-badge-unlocks.{mainTwitchUser.ListData[0].Id}",
                $"channel-points-channel-v1.{mainTwitchUser.ListData[0].Id}",
                $"channel-subscribe-events-v1.{mainTwitchUser.ListData[0].Id}",
                $"chat_moderator_actions.{mainTwitchUser.ListData[0].Id}.{mainTwitchUser.ListData[0].Id}",
                $"automod-queue.{mainTwitchUser.ListData[0].Id}.{mainTwitchUser.ListData[0].Id}",
                $"user-moderation-notifications.{mainTwitchUser.ListData[0].Id}.{mainTwitchUser.ListData[0].Id}",
                $"whispers.{mainTwitchUser.ListData[0].Id}"
                };
            request._Data.Topics = tpsSystem.topics;
            request._Data.AuthToken = accessToken;

            var json = JsonConvert.SerializeObject(request);
            Debug.Log($"{"PubSub Event Request".ColorString(ColorType.PubSubSend)} {json}");

            var encoded = Encoding.UTF8.GetBytes(json);
            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
            await pubSubWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}