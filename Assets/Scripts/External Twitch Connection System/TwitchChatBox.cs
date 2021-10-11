namespace PierreARNAUDET.TwitchChat
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;

    public class TwitchChatBox : MonoBehaviour
    {
        [SerializeField]
        private Text chatBox;

        public void Display(string author, string message)
        {
            var messageToShift = String.Format("{0}: {1}", author, message);
            TwitchData.messages = Shift(TwitchData.messages, messageToShift);
            chatBox.text = "Chat :" + "\n";
            foreach (string messageToDisplay in TwitchData.messages)
            {
                chatBox.text = chatBox.text + messageToDisplay + "\n";
            }
        }

        public string[] Shift(string[] oldMessages, string newMessage)
        {
            var newMessages = new string[TwitchData.messages.Length];
            Array.Copy(oldMessages, 1, newMessages, 0, oldMessages.Length - 1);
            newMessages[newMessages.Length - 1] = newMessage;
            return newMessages;
        }
    }
}