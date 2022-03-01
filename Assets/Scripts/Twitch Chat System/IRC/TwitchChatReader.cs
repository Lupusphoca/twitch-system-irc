namespace PierreARNAUDET.TwitchChat
{
    using UnityEngine;

    using static PierreARNAUDET.TwitchChat.TwitchData;

    public class TwitchChatReader : MonoBehaviour
    {
        [Header("Required parameters")]
        [SerializeField] UniqueAuthorsParameters uniqueAuthorsParameters;

        private void Update()
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                ReadChat();
            }
        }

        private void ReadChat()
        {
            if (tcpClient.Available > 0)
            {
                var message = streamReader.ReadLine();
                //Debug.Log(message);

                //* Twitch sends a PING message every 5 minutes or so. We MUST respond back with PONG or we will be disconnected.
                if (message.Contains("PING"))
                {
                    streamWriter.WriteLine("PONG");
                    streamWriter.Flush();
                    return;
                }

                //! A message look like this
                //! :loufhok!loufhok@loufhok.tmi.twitch.tv PRIVMSG #loufhok :Hello this is my message !
                if (message.Contains("PRIVMSG"))
                {
                    //* Get author name
                    var splitPoint = message.IndexOf("!"); //https://mzl.la/3AtsXtp
                    var author = message.Substring(0, splitPoint);
                    author = author.Substring(1);

                    //* Users message.
                    splitPoint = message.IndexOf(":", 1);
                    message = message.Substring(splitPoint + 1);

                    if (message.StartsWith(commandPrefix))
                    {
                        //* Get the first word.
                        int index = message.IndexOf(" ");
                        string command = index > -1 ? message.Substring(0, index) : message;

                        TwitchCommandData twitchCommandData = new TwitchCommandData();
                        twitchCommandData.Author = author;
                        twitchCommandData.Message = message;

                        commandCollection.ExecuteCommand(
                            command,
                            twitchCommandData);
                    }

                    uniqueAuthorsParameters.VerifyAuthorState(author, message);
                }
            }
        }
    }
}