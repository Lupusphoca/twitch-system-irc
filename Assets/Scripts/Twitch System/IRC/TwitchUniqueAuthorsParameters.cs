namespace PierreARNAUDET.TwitchUtilitary
{
    using System;

    using UnityEngine;
    
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;
    using static PierreARNAUDET.TwitchUtilitary.TwitchGlobalBadgesData;
    using static PierreARNAUDET.TwitchUtilitary.TwitchIRCAuthorParametersData;
    
    public static class TwitchUniqueAuthorsParameters
    {
        public static AuthorParameters VerifyAuthorState(string author, UserPrivmsgMetadata userData)
        {
            if (authors.TryGetValue(author, out AuthorParameters value))
            {
                return value;
            }
            else
            {
                return AddAuthorToDictionary(author, userData); //* Add author to the register list.
            }
        }

        public static AuthorParameters GetAuthorParameters(string author)
        {
            if (authors.TryGetValue(author, out AuthorParameters value))
            {
                return value;
            }
            else
            {
                return new AuthorParameters();
            }
        }

        #region New register
        private static AuthorParameters AddAuthorToDictionary(string author, UserPrivmsgMetadata userData)
        {
            var authorParameters = new AuthorParameters();
            authorParameters.UserPrivmsgMetaData = userData;
            authorParameters.Username = author;
            authorParameters.urlBadges = GetURLBadgesAuthor(author, authorParameters.UserPrivmsgMetaData.Badges);
            authors.Add(author, authorParameters);
            Debug.Log("New register !".ColorString(ColorType.CommonConsole));
            return authorParameters;
        }


        private static string[] GetURLBadgesAuthor(string author, string badges)
        {
            var badgeSplit = badges.Split(",");
            var urlBadges = new string[badgeSplit.Length];
            for (int i = 0; i < badgeSplit.Length; i++)
            {
                var badgeInfo = badgeSplit[i].Split("/");
                for (int j = 0; j < globalBadges.ListData.Count; j++)
                {
                    if (globalBadges.ListData[j].SetId == badgeInfo[0])
                    {
                        for (int k = 0; k < globalBadges.ListData[j].Versions.Count; k++)
                        {
                            if (globalBadges.ListData[j].Versions[k].Id == badgeInfo[1])
                            {
                                urlBadges[i] = globalBadges.ListData[j].Versions[k].ImageUrl4x;
                            }
                        }
                    }
                }
            }
            return urlBadges;
        }

        [Obsolete]
        private static string GetRandomColor()
        {
            var random = new System.Random();
            return "<color=" + String.Format("#{0:X6}", random.Next(0x1000000)) + ">";
        }
        #endregion
    }
}