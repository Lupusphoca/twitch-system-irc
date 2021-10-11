namespace PierreARNAUDET.TwitchChat
{
    using UnityEngine;

    using static PierreARNAUDET.TwitchChat.TwitchData;

    public class DebugAutomaticConnection : MonoBehaviour
    {
        [SerializeField]
        private TwitchChatConnection twitchChatConnection;

        [SerializeField]
        private string password;

        [SerializeField]
        private string username;

        [SerializeField]
        private string channelName;

        private void Start()
        {
            if (password != string.Empty && username != string.Empty && channelName != string.Empty)
            {
                twitchCredentials.Password = password;
                twitchCredentials.Username = username;
                twitchCredentials.ChannelName = channelName;
                twitchChatConnection.Connect();
            }
        }
    }
}