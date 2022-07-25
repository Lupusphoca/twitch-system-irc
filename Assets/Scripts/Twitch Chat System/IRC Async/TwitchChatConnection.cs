namespace PierreARNAUDET.TwitchChat
{
    using System.IO;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using UnityEngine;

    using AsyncAwaitBestPractices;

    using PierreARNAUDET.Core.Attributes;
    using static PierreARNAUDET.TwitchChat.TwitchData;

    class TwitchChatConnection : MonoBehaviour
    {
        [Data]
        [SerializeField] TwitchChatReader twitchChatReader;

        private string ip = "irc.chat.twitch.tv";
        private int port = 6667;

        public async Task Connect()
        {
            var password = twitchCredentials.Password;
            var username = twitchCredentials.Username.ToLower();
            var channelName = twitchCredentials.ChannelName.ToLower();

            Debug.Log($"Check : {password} + {username} + {channelName}");

            if (password != string.Empty && username != string.Empty && channelName != string.Empty)
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(ip, port);
                Debug.Log($"Twitch Client Static Value : {tcpClient}");
                commandCollection = new TwitchCommandCollection(); //? Here ?

                streamReader = new StreamReader(tcpClient.GetStream());
                streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };
                // NewLine = "\r\n" automatically puts \r\n after every line which marks the end of a message
                // AutoFlush = true will call streamWriter.Flush(); after every write call
                // So instead of doing
                // await streamWriter.WriteLineAsync($"PASS {password}\r\n");
                // await streamWriter.FlushAsync();
                // All we need now is : await streamWriter.WriteLineAsync($"PASS {password}");

                await streamWriter.WriteLineAsync($"PASS {password}");
                await streamWriter.WriteLineAsync($"NICK {username}");
                await streamWriter.WriteLineAsync($"JOIN #{channelName}");
                // await streamWriter.WriteLineAsync($"PRIVMSG #{channelName} :Hey I just started my bot !");

                twitchChatReader.Read().SafeFireAndForget(); //! Because Read() is a while(true) loop, next steps will never be launched in this function.
                Debug.Log("Does code reach this place ? Let's have a look at it.");

                await streamWriter.WriteLineAsync($"CAP REQ :twitch.tv/tags twitch.tv/commands");
            }
        }
    }
}
