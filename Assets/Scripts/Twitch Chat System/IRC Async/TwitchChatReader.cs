namespace PierreARNAUDET.TwitchChat
{
    using System.Threading.Tasks;

    using UnityEngine;

    using static PierreARNAUDET.TwitchChat.TwitchData;

    public class TwitchChatReader : MonoBehaviour
    {
        [Header("Required parameters")]
        [SerializeField] TwitchChatBox twitchChatBox;

        public async Task Read()
        {
            Debug.Log($"TCP Client State : {tcpClient.Connected}");
            if (tcpClient != null && tcpClient.Connected)
            {
                while (true)
                {
                    var line = await streamReader.ReadLineAsync(); //* while(true) work because of the await here that pause and wait untill it receives a message
                    Debug.Log(line);
                    // Example message :
                    // :mytwitchchannel!mytwitchchannel@mytwitchchannel.tmi.twitch.tv PRIVMSG #mytwitchchannel :test

                    // 0 TAG DATA - @badge-info=;badges=moderator/1;client-nonce=64c78c9db33f705d582a71514fd5fa77;color=;display-name=lupusphoca;emotes=;first-msg=0;flags=;id=32ffee59-3e79-46a1-b132-454f8e4474f4;mod=1;returning-chatter=0;room-id=45705640;subscriber=0;tmi-sent-ts=1657192524946;turbo=0;user-id=762695180;user-type=mod
                    // 1 AUTHOR INFO - :lupusphoca!lupusphoca@lupusphoca.tmi.twitch.tv
                    // 2 TAG TYPE - PRIVMSG
                    // 3 CHANNEL INFO - #channelName (Not in GLOBALUSERSTATE)
                    // 4 MESSAGE - :message (Not in CLEARCHAT, GLOBALUSERSTATE)

                    var split = line.Split(' ');

                    if (line.StartsWith("PING"))
                    {
                        await streamWriter.WriteLineAsync("PONG"); //* $"PONG {split[1]}"
                    }

                    if (split.Length > 1 && split[2] == "PRIVMSG") // Verify that the second element split is PRIVMSG
                    {
                        //* Get author.
                        var exclamationPointPosition = split[1].IndexOf("!"); // We get the exclamation point position in split[0] https://mzl.la/3AtsXtp
                        var author = split[1].Substring(1, exclamationPointPosition - 1); // Get string from character at place 1 to exclamationPointPosition - 1 which is the username

                        //* Get user message.
                        var secondColonPosition = line.IndexOf(':', 1); // We get the second colon position in line
                        var message = line.Substring(secondColonPosition + 1); // Get string from secondColonPosition + 1 to the end

                        Debug.Log($"{author} said '{message}'");

                        //* Execute command if it's seem's to be one.
                        if (message.StartsWith(commandPrefix))
                        {
                            //* Get first word
                            var index = message.IndexOf(" ");
                            var command = index > -1 ? message.Substring(0, index) : message;

                            //! After badges and roles are implemented, add commands permissions and verify that the author role can use this command.
                            var TwitchCommandData = new TwitchCommandData();
                            TwitchCommandData.Author = author;
                            TwitchCommandData.Message = message;
                            commandCollection.ExecuteCommand(command, TwitchCommandData);
                        }

                        var authorParameters = UniqueAuthorsParameters.VerifyAuthorState(author);
                        twitchChatBox.Display(authorParameters.DisplayUsername, message);
                    }
                }
            }
        }
    }
}