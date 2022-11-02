namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.IO;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    using UnityEngine;

    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;

    class TwitchPubSubReceive
    {
        private TwitchPubSubSystem tpsSystem;

        protected internal TwitchPubSubReceive(TwitchPubSubSystem tpsSystem)
        {
            this.tpsSystem = tpsSystem;
        }

        protected internal async Task Receive()
        {
            do
            {
                var buffer = new byte[tpsSystem.ReceiveChunkSize];
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
                    tpsSystem.LogStatus("Receive", buffer, data, totalBytesReceived);
                }
            } while (pubSubWebSocket.State == WebSocketState.Open);

            Debug.Log("Receive process ended ...".ColorString(ColorType.Error));
        }

        //TODO : Resend analysed data into TwitchPubSubSystem (or a new MonoBehaviour script like TwitchPubSubOutEvent ?) that will send them into UnityEvent
        protected internal void AnalyseReceiveData(string data)
        {
            var parsedObject = JObject.Parse(data);
            var typeJson = parsedObject["type"].ToString();
            Debug.Log("PubSub Type of Receive Data ".ColorString(ColorType.CommonConsole) + typeJson);

            switch (typeJson)
            {
                case "PONG":
                    tpsSystem.pingPongValidation = true;
                    break;
                case "RECONNECT":
                    var tpsReconnect = new TwitchPubSubReconnect(tpsSystem);
                    _ = tpsReconnect.Reconnect();
                    break;
                case "RESPONSE":
                    break;
                case "MESSAGE":
                    break;
                case "reward-redeemed":
                    break;
                default:
                    break;
            }
        }
    }
}