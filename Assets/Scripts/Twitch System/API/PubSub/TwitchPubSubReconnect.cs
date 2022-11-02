namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    using UnityEngine;

    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;

    class TwitchPubSubReconnect
    {
        private TwitchPubSubSystem tpsSystem;

        protected internal TwitchPubSubReconnect(TwitchPubSubSystem tpsSystem)
        {
            this.tpsSystem = tpsSystem;
        }

        protected internal async Task Reconnect()
        {
            await pubSubWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Reconnection process ...", CancellationToken.None);
            pubSubWebSocket.Dispose();
            await Task.Delay(TimeSpan.FromSeconds(5));
            Debug.Log("Restart PubSub connection".ColorString(ColorType.Error));
            tpsSystem.Connect();
        }
    }
}