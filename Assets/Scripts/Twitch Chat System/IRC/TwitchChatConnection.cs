namespace PierreARNAUDET.TwitchChat
{
    using System.IO;
    using System.Net.Sockets;

    using UnityEngine;

    using static PierreARNAUDET.TwitchChat.TwitchData;

    public class TwitchChatConnection : MonoBehaviour
    {
        public void Connect()
        {
            var password = twitchCredentials.Password;
            var username = twitchCredentials.Username.ToLower();
            var channelName = twitchCredentials.ChannelName.ToLower();

            if (password != string.Empty && username != string.Empty && channelName != string.Empty)
            {
                tcpClient = new TcpClient("irc.chat.twitch.tv", 6667);
                Debug.Log($"Twitch Client Static Value : {tcpClient}");
                commandCollection = new TwitchCommandCollection();

                streamReader = new StreamReader(tcpClient.GetStream());
                streamWriter = new StreamWriter(tcpClient.GetStream());

                streamWriter.WriteLine("PASS " + password);
                streamWriter.WriteLine("NICK " + username);
                streamWriter.WriteLine("USER " + username + " 8 * :" + username);
                streamWriter.WriteLine("JOIN #" + channelName);
                streamWriter.WriteLineAsync($"PRIVMSG #{channelName} :Hey I just started my IRC bot");
                streamWriter.Flush();
            }
        }
    }
}