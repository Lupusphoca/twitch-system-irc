namespace PierreARNAUDET.TwitchChat
{
    using System.IO;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;

    class Main : MonoBehaviour
    {
        [Settings]
        [Tooltip("OAuth key of the wanted account")]
        [SerializeField] string passwordBot; //! Don't share those keys
        [Tooltip("Twitch channel to connect to")]
        [SerializeField] string twitchChannel;

        private string ip = "irc.chat.twitch.tv";
        private int port = 6667;
        private string username = "PierrotBot"; //? Weird because it could be anything

        public async Task Run()
        {
            var twitch = new TcpClient();
            await twitch.ConnectAsync(ip, port);

            var streamReader = new StreamReader(twitch.GetStream());
            var streamWriter = new StreamWriter(twitch.GetStream()) {NewLine = "\r\n", AutoFlush = true };
            // NewLine = "\r\n" automatically puts \r\n after every line which marks the end of a message
            // AutoFlush = true will call streamWriter.Flush(); after every write call
            // So instead of doing
            // await streamWriter.WriteLineAsync($"PASS {password}\r\n");
            // await streamWriter.FlushAsync();
            // All we need now is : await streamWriter.WriteLineAsync($"PASS {password}");

            await streamWriter.WriteLineAsync($"PASS {passwordBot}");
            await streamWriter.WriteLineAsync($"NICK {username}");
            await streamWriter.WriteLineAsync($"JOIN #{twitchChannel}");

            //await streamWriter.WriteLineAsync($"PRIVMSG #{twitchChannel} :Hey I just started my IRC bot");

            while (true)
            {
                var line = await streamReader.ReadLineAsync(); // Pause and wait untill it receives a message

                // Example message :
                // :mytwitchchannel!mytwitchchannel@mytwitchchannel.tmi.twitch.tv PRIVMSG #mytwitchchannel :test

                var split = line.Split(' ');
                if (line.StartsWith("PING"))
                {
                    await streamWriter.WriteLineAsync("PONG"); //* $"PONG {split[1]}"
                }

                if (split.Length > 1 && split[1] == "PRIVMSG") // Verify that the second element split is PRIVMSG
                {
                    var exclamationPointPosition = split[0].IndexOf("!"); // We get the exclamation point position in split[0]
                    var username = split[0].Substring(1, exclamationPointPosition - 1); // Get string from character at place 1 to exclamationPointPosition - 1 which is the username

                    var secondColonPosition = line.IndexOf(':', 1); // We get the second colon position in line
                    var message = line.Substring(secondColonPosition + 1); // Get string from secondColonPosition + 1 to the end
                    Debug.Log($"{username} said '{message}'");
                }
            }
        }
    }
}
