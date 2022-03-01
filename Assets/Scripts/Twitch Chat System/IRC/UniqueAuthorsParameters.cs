namespace PierreARNAUDET.TwitchChat
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class UniqueAuthorsParameters : MonoBehaviour
    {
        [Header("Required parameters")]
        [SerializeField] TwitchChatBox twitchChatBox;

        struct AuthorParameters
        {
            public string username;
            public string color;
            public string DisplayUsername { get => color + username + "</color>"; } //* Used to replace author string. Combinaison of username and color.
        }

        List<AuthorParameters> authors = new List<AuthorParameters>();

        protected internal void VerifyAuthorState(string author, string message)
        {
            foreach (AuthorParameters authorParameters in authors)
            {
                if (authorParameters.username == author)
                {
                    twitchChatBox.Display(authorParameters.DisplayUsername, message);
                    Debug.Log("Already register !");
                    return; //* Stop loop if already exist in list.
                }
            }
            AddAuthorToList(author, message); //* Add author to the register list.
            Debug.Log("New register !");
        }

        private void AddAuthorToList(string author, string message)
        {
            var authorParameters = new AuthorParameters();
            authorParameters.username = author;
            authorParameters.color = GetRandomColor();
            authors.Add(authorParameters);
            twitchChatBox.Display(authorParameters.DisplayUsername, message);
        }

        private string GetRandomColor()
        {
            var random = new System.Random();
            return "<color=" + String.Format("#{0:X6}", random.Next(0x1000000)) + ">";
        }
    }
}