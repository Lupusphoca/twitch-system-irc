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

        #region Display Informations
        public static TwitchInformationBox twitchInformationBox;
        public static string[] informations;
        #endregion
    }
}