namespace PierreARNAUDET.TwitchChat
{
    using System.Threading.Tasks;
    using UnityEngine;
    
    using PierreARNAUDET.Core.Attributes;
    using static PierreARNAUDET.TwitchChat.TwitchData;

    public class DebugAutomaticConnection : MonoBehaviour
    {
        [Data]
        [SerializeField] private TwitchChatConnection twitchChatConnection;
        [SerializeField] private string password;
        [SerializeField] private string username;
        [SerializeField] private string channelName;

        private async Task Start()
        {
            if (password != string.Empty && username != string.Empty && channelName != string.Empty)
            {
                twitchCredentials.Password = password;
                twitchCredentials.Username = username;
                twitchCredentials.ChannelName = channelName;
                await twitchChatConnection.Connect();
            }
        }
    }
}