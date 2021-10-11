﻿namespace PierreARNAUDET.TwitchChat
{
    using UnityEngine;
    using UnityEngine.UI;

    public class TwitchChatSettingsUI : MonoBehaviour
    {
        public InputField PasswordInput;
        public InputField UsernameInput;
        public InputField ChannelNameInput;
        public TwitchChat TwitchChat;

        void Start()
        {
            if (PlayerPrefs.HasKey("TwitchOAuthPass"))
            {
                var password = PlayerPrefs.GetString("TwitchOAuthPass");
                PasswordInput.text = password;
            }
        }

        public void Connect()
        {
            TwitchCredentials credentials = new TwitchCredentials
            {
                ChannelName = ChannelNameInput.text.ToLower(),
                Username = UsernameInput.text.ToLower(),
                Password = PasswordInput.text
            };
            TwitchChat.Connect(credentials, new CommandCollection());
        }

    }
}