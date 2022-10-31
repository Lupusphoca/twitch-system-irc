namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;

    using Newtonsoft.Json;

    public static class TwitchStaticData
    {
        #region Common Data
        public static TwitchCredentials twitchCredentials;
        public struct TwitchCredentials
        {
            [JsonProperty("client_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ClientId { get; set; }
            [JsonProperty("redirect_url", NullValueHandling = NullValueHandling.Ignore)]
            public string RedirectURL { get; set; }
            [JsonProperty("scopes", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Scopes;
            [JsonProperty("channel_connection", NullValueHandling = NullValueHandling.Ignore)]
            public string ChannelConnection { get; set; }
            [JsonProperty("bot_name", NullValueHandling = NullValueHandling.Ignore)]
            public string BotName { get; set; }

        }
        public static bool forceVerify;
        public static string accessToken;
        #endregion

        #region User
        public static TwitchUser twitchUser;
        public struct TwitchUser
        {
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public List<TwitchUser.Data> ListData;
            public struct Data
            {
                [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
                public string Id { get; set; }
                [JsonProperty("login", NullValueHandling = NullValueHandling.Ignore)]
                public string Login { get; set; }
                [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
                public string DisplayName { get; set; }
                [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
                public string Type { get; set; }
                [JsonProperty("broadcaster_type", NullValueHandling = NullValueHandling.Ignore)]
                public string BroadcasterType { get; set; }
                [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
                public string Description { get; set; }
                [JsonProperty("profile_image_url", NullValueHandling = NullValueHandling.Ignore)]
                public string ProfileImageUrl { get; set; }
                [JsonProperty("offline_image_url", NullValueHandling = NullValueHandling.Ignore)]
                public string OfflineImageUrl { get; set; }
                [JsonProperty("view_count", NullValueHandling = NullValueHandling.Ignore)]
                public int ViewCount { get; set; }
                [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
                public string Email { get; set; }
                [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
                public string CreatedAt { get; set; }
            }

        }
        #endregion

        #region Commands
        public static readonly string commandPrefix = "!";
        public static readonly string commandMessage = "message";

        public static TwitchCommandCollection commandCollection = new TwitchCommandCollection();
        public struct TwitchCommandData
        {
            public string Author;
            public string Message;
        }
        #endregion

        #region IRC Network
        public static TcpClient tcpClient;
        public static StreamReader streamReader;
        public static StreamWriter streamWriter;
        #endregion

        #region Display Chat
        public static string[] messages;
        #endregion

        #region API/GlobalBadges
        public static GlobalBadges globalBadges;
        public struct GlobalBadges
        {
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public List<GlobalBadges.Data> ListData;

            public struct Data
            {
                [JsonProperty("set_id", NullValueHandling = NullValueHandling.Ignore)]
                public string SetId { get; set; }
                [JsonProperty("versions", NullValueHandling = NullValueHandling.Ignore)]
                public List<Versions> Versions;
            }

            public struct Versions
            {
                [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
                public string Id { get; set; }
                [JsonProperty("image_url_1x", NullValueHandling = NullValueHandling.Ignore)]
                public string ImageUrl1x { get; set; }
                [JsonProperty("image_url_2x", NullValueHandling = NullValueHandling.Ignore)]
                public string ImageUrl2x { get; set; }
                [JsonProperty("image_url_4x", NullValueHandling = NullValueHandling.Ignore)]
                public string ImageUrl4x { get; set; }
            }
        }
        #endregion

        #region IRC Author Data
        public static Dictionary<string, AuthorParameters> authors2 = new Dictionary<string, AuthorParameters>();
        public static List<AuthorParameters> authors = new List<AuthorParameters>();
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