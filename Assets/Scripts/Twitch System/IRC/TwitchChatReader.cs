namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.Threading.Tasks;

    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;
    
    public class TwitchChatReader : MonoBehaviour
    {
        [Data]
        [SerializeField] TwitchChatBox twitchChatBox;

        private string line;

        public async Task Read()
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                while (true)
                {
                    // Example message :
                    // :mytwitchchannel!mytwitchchannel@mytwitchchannel.tmi.twitch.tv PRIVMSG #mytwitchchannel :test
                    line = await streamReader.ReadLineAsync(); //* while(true) work because of the await here that pause and wait untill it receives a message
                    Debug.Log($"{"IRC".ColorString(ColorType.IRC)} {line}");
                    if (line != string.Empty)
                    {
                        var lineSplit = line.Split(' ');
                        // Commons
                        // lineSplit [0] TAG DATA - @badge-info=;badges=moderator/1;client-nonce=64c78c9db33f705d582a71514fd5fa77;color=;display-name=lupusphoca;emotes=;first-msg=0;flags=;id=32ffee59-3e79-46a1-b132-454f8e4474f4;mod=1;returning-chatter=0;room-id=45705640;subscriber=0;tmi-sent-ts=1657192524946;turbo=0;user-id=762695180;user-type=mod
                        // lineSplit [1] AUTHOR INFO - :lupusphoca!lupusphoca@lupusphoca.tmi.twitch.tv 
                        // lineSplit [2] TAG TYPE - PRIVMSG || The only data which is in every message at the same place

                        // Specifics
                        // lineSplit [3] CHANNEL INFO - #channelName (Not in GLOBALUSERSTATE) || Can be replace by foo in WHISPER
                        // lineSplit [4] MESSAGE - :message (Not in CLEARCHAT, GLOBALUSERSTATE, ROOMSTATE, USERSTATE, not in all USERNOTICE) (if cheer bits message will be :cheer100 or whatever is the amount)

                        if (line.StartsWith("PING"))
                        {
                            await streamWriter.WriteLineAsync($"PONG :tmi.twitch.tv"); //* $"PONG {split[1]}" {lineSplit[1]}
                            // await streamWriter.FlushAsync();
                            // Debug.Log("Ping response Pong ...");
                            // return;
                        }
                        else if (lineSplit.Length > 1)
                        {
                            TagSplit(lineSplit);
                        }
                    }
                }
            }
        }

        private void TagSplit(string[] lineSplit)
        {
            switch (lineSplit[2])
            {
                case "CLEARCHAT":
                    break;
                case "CLEARMSG":
                    break;
                case "GLOBALUSERSTATE":
                    break;
                case "NOTICE":
                    break;
                case "PRIVMSG":
                    //* Get author.
                    var exclamationPointPosition = lineSplit[1].IndexOf("!"); // We get the exclamation point position in lineSplit[0] https://mzl.la/3AtsXtp
                    var author = lineSplit[1].Substring(1, exclamationPointPosition - 1); // Get string from character at place 1 to exclamationPointPosition - 1 which is the username

                    //* Get user message.
                    var message = line.Split(':')[2];
                    Debug.Log($"{author.ColorString(ColorType.IRC)} said <color=white>'{message}'</color>");

                    //* Stock metadata info of the user
                    var infos = lineSplit[0].Split(';');

                    var userData = new UserPrivmsgMetadata();
                    foreach (string info in infos)
                    {
                        var splitInfo = info.Split('=');

                        switch (splitInfo[0])
                        {
                            case "badge-info":
                                userData.BadgeInfo = splitInfo[1];
                                break;
                            case "badges":
                                userData.Badges = splitInfo[1];
                                break;
                            case "bits":
                                var outBits = 0;
                                var parseBits = Int32.TryParse(splitInfo[1], out outBits);
                                if (parseBits)
                                {
                                    userData.Bits = outBits;
                                }
                                else
                                {
                                    Debug.LogWarning("User data 'Bits' cannot be parsed.");
                                }
                                break;
                            case "color":
                                userData.Color = splitInfo[1];
                                break;
                            case "display-name":
                                userData.DisplayName = splitInfo[1];
                                break;
                            case "emotes":
                                userData.Emotes = splitInfo[1];
                                break;
                            case "id":
                                userData.Id = splitInfo[1];
                                break;
                            case "mod":
                                var outMod = false;
                                var parseMod = Boolean.TryParse(splitInfo[1], out outMod);
                                if (parseMod)
                                {
                                    userData.Mod = outMod;
                                }
                                else
                                {
                                    Debug.LogWarning("User data 'Mod' cannot be parsed.");
                                }
                                break;
                            case "subscriber":
                                var outSubcriber = false;
                                var parseSubscriber = Boolean.TryParse(splitInfo[1], out outSubcriber);
                                if (parseSubscriber)
                                {
                                    userData.Subscriber = outSubcriber;
                                }
                                else
                                {
                                    Debug.LogWarning("User data 'Subscriber' cannot be parsed.");
                                }
                                break;
                            case "turbo":
                                var outTurbo = false;
                                var parseTurbo = Boolean.TryParse(splitInfo[1], out outTurbo);
                                if (parseTurbo)
                                {
                                    userData.Turbo = outTurbo;
                                }
                                else
                                {
                                    Debug.LogWarning("User data 'Turbo' cannot be parsed.");
                                }
                                break;
                            case "user-id":
                                userData.UserId = splitInfo[1];
                                break;
                            case "user-type":
                                userData.UserType = splitInfo[1];
                                break;
                            case "vip":
                                var outVip = false;
                                var parseVip = Boolean.TryParse(splitInfo[1], out outVip);
                                if (parseVip)
                                {
                                    userData.Vip = outVip;
                                }
                                else
                                {
                                    Debug.LogWarning("User data 'Vip' cannot be parsed.");
                                }
                                break;
                            default:
                                break;
                        }
                    }

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

                    var authorParameters = TwitchUniqueAuthorsParameters.VerifyAuthorState(author, userData);
                    twitchChatBox.Display(authorParameters.DisplayUsername, message);
                    break;
                case "ROOMSTATE":
                    break;
                case "USERNOTICE":
                    break;
                case "USERSTATE":
                    break;
                case "WHISPER":
                    break;
                default:
                    break;
            }
        }
    }
}