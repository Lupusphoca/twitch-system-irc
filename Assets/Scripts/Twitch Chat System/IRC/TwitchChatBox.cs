namespace PierreARNAUDET.TwitchChat
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;

    using TMPro;

    using PierreARNAUDET.Core.Attributes;

    public class TwitchChatBox : MonoBehaviour
    {
        [Header("Required parameters")]
        [SerializeField, ConditionalHide("useTextMeshPro", true, true)] Text textChatBox;
        [SerializeField, ConditionalHide("useTextMeshPro", true, false)] TextMeshProUGUI textMeshProChatBox;

        [Header("Settings")]
        [SerializeField] bool useTextMeshPro;

        protected internal void Display(string author, string message)
        {

            var messageToShift = String.Format("{0} : {1}", author, message);
            TwitchData.messages = Shift(TwitchData.messages, messageToShift);
            var text = string.Empty;

            text = "Chat :" + "\n";
            foreach (string messageToDisplay in TwitchData.messages)
            {
                text = text + messageToDisplay + "\n";
            }

            if (useTextMeshPro)
            {
                textMeshProChatBox.SetText(text);
            }
            else
            {
                textChatBox.text = text;
            }
        }

        private string[] Shift(string[] oldMessages, string newMessage)
        {
            string[] newMessages = new string[TwitchData.messages.Length];
            Array.Copy(oldMessages, 1, newMessages, 0, oldMessages.Length - 1);
            newMessages[newMessages.Length - 1] = newMessage;
            return newMessages;
        }
    }
}