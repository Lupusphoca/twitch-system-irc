namespace PierreARNAUDET.TwitchChat
{
    using System;
    using System.Net.Sockets;
    using System.IO;

    using UnityEngine;
    using UnityEngine.UI;

    public class TwitchChat : MonoBehaviour
    {
        /* [SerializeField]
        private string username;

        [SerializeField]
        private string password; //Get the channel password from https://twitchapps.com/tmi 

        [SerializeField]
        private string channelName; */

        [SerializeField]
        private Text chatBox;

        [SerializeField]
        private string[] messages;

        public static TwitchChat Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TwitchChat();
                }
                return instance;
            }
        }
        private static TwitchChat instance;

        private CommandCollection commands;
        private TcpClient twitchClient;
        private StreamReader reader;
        private StreamWriter writer;

        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        void Update()
        {
            if (twitchClient != null && twitchClient.Connected)
            {
                ReadChat();
            }
        }

        public void SetNewCommandCollection(CommandCollection newCommands)
        {
            commands = newCommands;
        }

        public void Connect(TwitchCredentials credentials, CommandCollection newCommands)
        {
            commands = newCommands;
            twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
            reader = new StreamReader(twitchClient.GetStream());
            writer = new StreamWriter(twitchClient.GetStream());

            writer.WriteLine("PASS " + credentials.Password);
            writer.WriteLine("NICK " + credentials.Username);
            writer.WriteLine("USER " + credentials.Username + " 8 * :" + credentials.Username);
            writer.WriteLine("JOIN #" + credentials.ChannelName);
            writer.Flush();
        }

        private void ReadChat()
        {
            if (twitchClient.Available > 0)
            {
                var message = reader.ReadLine();
                Debug.Log(message);

                //* Twitch sends a PING message every 5 minutes or so. We MUST respond back with PONG or we will be disconnected.
                if (message.Contains("PING"))
                {
                    writer.WriteLine("PONG");
                    writer.Flush();
                    return;
                }

                if (message.Contains("PRIVMSG"))
                {
                    var splitPoint = message.IndexOf("!", 1);
                    var author = message.Substring(0, splitPoint);
                    author = author.Substring(1);

                    //* Users message.
                    splitPoint = message.IndexOf(":", 1);
                    message = message.Substring(splitPoint + 1);

                    if (message.StartsWith(TwitchCommands.CmdPrefix))
                    {
                        //* Get the first word.
                        int index = message.IndexOf(" ");
                        string command = index > -1 ? message.Substring(0, index) : message;
                        commands.ExecuteCommand(
                            command,
                            new TwitchCommandData
                            {
                                Author = author,
                                Message = message
                            });
                    }
                    //chatBox.text = chatBox.text + "\n" + String.Format("{0}: {1}", author, message);
                    var messageToShift = String.Format("{0}: {1}", author, message);
                    messages = Shift(messages, messageToShift);
                    chatBox.text = "Chat :" + "\n";
                    foreach (string messageToDisplay in messages)
                    {
                        chatBox.text = chatBox.text + messageToDisplay + "\n";
                    }
                }
            }
        }

        public string[] Shift(string[] oldMessages, string newMessage)
        {
            var newMessages = new string[messages.Length];
            Array.Copy(oldMessages, 1, newMessages, 0, oldMessages.Length - 1);
            newMessages[newMessages.Length - 1] = newMessage;
            return newMessages;
        }
    }
}