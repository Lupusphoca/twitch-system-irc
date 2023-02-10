namespace PierreARNAUDET.TwitchUtilitary
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;

    using TMPro;

    using PierreARNAUDET.Core.Attributes;

    public class TwitchInformationBox : MonoBehaviour
    {
        [Data]
        [SerializeField, ConditionalHide("useTextMeshPro", true, true)] Text textChatBox;
        [SerializeField, ConditionalHide("useTextMeshPro", true, false)] TextMeshProUGUI textMeshProChatBox;

        [Settings]
        [SerializeField] bool useTextMeshPro;

        protected internal void Display(string element)
        {
            TwitchStaticData.informations = Shift(TwitchStaticData.informations, element);

            var text = string.Empty;
            foreach (string elementToDisplay in TwitchStaticData.informations)
            {
                text = text + elementToDisplay + "\n";
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

        private string[] Shift(string[] oldElements, string newElement)
        {
            var newElements = new string[TwitchStaticData.informations.Length];
            Array.Copy(oldElements, 1, newElements, 0, oldElements.Length - 1);
            newElements[newElements.Length - 1] = newElement;
            return newElements;
        }
    }
}