namespace PierreARNAUDET.TwitchChat
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public static class UniqueAuthorsParameters
    {
        public struct AuthorParameters
        {
            public string username;
            public string color;
            public string DisplayUsername { get => color + username + "</color>"; } //* Used to replace author string. Combinaison of username and color.
        }

        public static List<AuthorParameters> authors = new List<AuthorParameters>();

        public static AuthorParameters VerifyAuthorState(string author)
        {
            foreach (AuthorParameters authorParameters in authors)
            {
                if (authorParameters.username == author)
                {
                    Debug.Log("Already register !");
                    return authorParameters; //* Stop loop if already exist in list.
                }
            }
            return AddAuthorToList(author); //* Add author to the register list.
        }

        #region New register
        private static AuthorParameters AddAuthorToList(string author)
        {
            var authorParameters = new AuthorParameters();
            authorParameters.username = author;
            authorParameters.color = GetRandomColor();
            authors.Add(authorParameters);
            Debug.Log("New register !");
            return authorParameters;
        }

        private static string GetRandomColor()
        {
            var random = new System.Random();
            return "<color=" + String.Format("#{0:X6}", random.Next(0x1000000)) + ">";
        }
        #endregion
    }
}