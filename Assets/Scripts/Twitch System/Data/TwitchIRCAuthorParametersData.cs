namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Collections.Generic;

    public static class TwitchIRCAuthorParametersData
    {
        #region IRC Author Data
        public static Dictionary<string, AuthorParameters> authors = new Dictionary<string, AuthorParameters>();
        public struct AuthorParameters
        {
            public string Username { get; set; }
            public UserPrivmsgMetadata UserPrivmsgMetaData { get; set; }
            public string DisplayUsername { get => "<color=" + UserPrivmsgMetaData.Color + ">" + UserPrivmsgMetaData.DisplayName + "</color>"; } //* Used to replace author string. Combinaison of username and color.
            public string[] urlBadges;
        }

        public struct UserPrivmsgMetadata
        {
            public string BadgeInfo { get; set; }
            public string Badges { get; set; }
            public int Bits { get; set; }
            public string Color { get; set; }
            public string DisplayName { get; set; }
            public string Emotes { get; set; }
            public string Id { get; set; }
            public bool Mod { get; set; }
            public bool Subscriber { get; set; }
            public bool Turbo { get; set; }
            public string UserId { get; set; }
            public string UserType { get; set; }
            public bool Vip { get; set; }
        }
        #endregion
    }
}