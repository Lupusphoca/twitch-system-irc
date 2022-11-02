
namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;

    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;

    class TwitchPubSubSend
    {
        private TwitchPubSubSystem tpsSystem;

        protected internal TwitchPubSubSend(TwitchPubSubSystem tpsSystem)
        {
            this.tpsSystem = tpsSystem;
        }

        protected internal async Task Send()
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
    }
}