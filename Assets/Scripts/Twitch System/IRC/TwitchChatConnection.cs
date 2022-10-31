namespace PierreARNAUDET.TwitchUtilitary
{
    using System.IO;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;

    public class TwitchChatConnection : MonoBehaviour
    {
        [Data]
        [SerializeField] TwitchChatReader twitchChatReader;

        private string ip = "irc.chat.twitch.tv";
        private int port = 6667;

        private void OnEnable()
        {
            TwitchUtilitaryManager.OnAccessTokenAuthorized -= Connect;
            TwitchUtilitaryManager.OnAccessTokenAuthorized += Connect;
        }

        private void Connect()
        {
            _ = ConnectTask();
        }

        public async Task ConnectTask()
        {
            var userAccessToken = accessToken;
            var botName = twitchCredentials.BotName.ToLower();
            var channelConnection = twitchCredentials.ChannelConnection.ToLower();
            if (userAccessToken != string.Empty && botName != string.Empty && channelConnection != string.Empty)
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(ip, port);
                streamReader = new StreamReader(tcpClient.GetStream());
                streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };
                // NewLine = "\r\n" automatically puts \r\n after every line which marks the end of a message
                // AutoFlush = true will call streamWriter.Flush(); after every write call
                // So instead of doing
                // await streamWriter.WriteLineAsync($"PASS {password}\r\n");
                // await streamWriter.FlushAsync();
                // All we need now is : await streamWriter.WriteLineAsync($"PASS {password}");

                await streamWriter.WriteLineAsync($"PASS oauth:{userAccessToken}");
                await streamWriter.WriteLineAsync($"NICK {botName}");

                await Task.Delay(2000); //* Only here to avoid connection problem

                await streamWriter.WriteLineAsync($"JOIN #{channelConnection}");
                await streamWriter.WriteLineAsync($"CAP REQ :twitch.tv/tags twitch.tv/commands"); //* Used to add more metadata to all the messages received from the API ? (https://dev.twitch.tv/docs/irc/capabilities)

                await Task.Delay(2000); //* Only here to avoid connection problem

                _ = twitchChatReader.Read();
            }
        }
    }
}
