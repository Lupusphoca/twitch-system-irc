namespace PierreARNAUDET.TwitchUtilitary
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;

    using TMPro;

    using PierreARNAUDET.Core.Attributes;

    public class TwitchChatBox : MonoBehaviour
    {
        [Data]
        [SerializeField, ConditionalHide("useTextMeshPro", true, true)] Text textChatBox;
        [SerializeField, ConditionalHide("useTextMeshPro", true, false)] TextMeshProUGUI textMeshProChatBox;

        [Settings]
        [SerializeField] bool useTextMeshPro;

        protected internal void Display(string author, string message)
        {
            var messageToShift = String.Format("{0} : {1}", author, message);

            //* Under here is the display badges in message system
            // var authorParameter = TwitchUniqueAuthorsParameters.GetAuthorParameters(author);
            // var urlFormat = authorParameter.urlBadges[0];
            // for (int i = 1; i < authorParameter.urlBadges.Length; i++)
            // {
            //     urlFormat = String.Format("{0} {1}", urlFormat, authorParameter.urlBadges[i]);
            // }
            // messageToShift = String.Format("{0} {1}", urlFormat, messageToShift);

            TwitchStaticData.messages = Shift(TwitchStaticData.messages, messageToShift);

            var text = "Chat :" + "\n";
            foreach (string messageToDisplay in TwitchStaticData.messages)
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
            var newMessages = new string[TwitchStaticData.messages.Length];
            Array.Copy(oldMessages, 1, newMessages, 0, oldMessages.Length - 1);
            newMessages[newMessages.Length - 1] = newMessage;
            return newMessages;
        }
    }
}