namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public static class TwitchGlobalEmotesData
    {
        #region API/GlobalEmotes
        public static GlobalEmotes globalEmotes;
        public struct GlobalEmotes
        {
            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public List<GlobalEmotes.GlobalEmote> ListData;

            public struct GlobalEmote
            {
                [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
                public string Id { get; set; }
                [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
                public string Name { get; set; }
                [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
                public Images Images { get; set; }
                [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
                public string[] Format { get; set; }
                [JsonProperty("scale", NullValueHandling = NullValueHandling.Ignore)]
                public string[] Scale { get; set; }
                [JsonProperty("theme_mode", NullValueHandling = NullValueHandling.Ignore)]
                public string[] ThemeMode { get; set; }
            }

            public struct Images
            {
                [JsonProperty("url_1x", NullValueHandling = NullValueHandling.Ignore)]
                public string Url1x { get; set; }
                [JsonProperty("url_2x", NullValueHandling = NullValueHandling.Ignore)]
                public string Url2x { get; set; }
                [JsonProperty("url_4x", NullValueHandling = NullValueHandling.Ignore)]
                public string Url4x { get; set; }
            }
        }

        [JsonProperty("template", NullValueHandling = NullValueHandling.Ignore)]
        public static string Template;
        #endregion
    }
}