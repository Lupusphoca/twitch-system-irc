
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

    class TwitchPubSubPingPong
    {
        private TwitchPubSubSystem tpsSystem;

        protected internal TwitchPubSubPingPong(TwitchPubSubSystem tpsSystem)
        {
            this.tpsSystem = tpsSystem;
        }

        protected internal async Task Ping()
        {
            tpsSystem.pingPongValidation = false;

            var timeSpan = TimeSpan.FromMinutes(4.5);
            await Task.Delay(timeSpan);

            var request = new TwitchPubSubSystem.PubSubType();
            request.Type = "PING";
            var json = JsonConvert.SerializeObject(request);
            Debug.Log($"{"PubSub Event PING verification".ColorString(ColorType.PubSubSend)} {json}");
            var encoded = Encoding.UTF8.GetBytes(json);
            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
            await pubSubWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            await Pong();
        }

        protected internal async Task Pong()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));

            if (tpsSystem.pingPongValidation)
            {
                _ = Ping();
            }
            else
            {
                var tpsReconnect = new TwitchPubSubReconnect(tpsSystem);
                _ = tpsReconnect.Reconnect();
            }
        }
    }
}