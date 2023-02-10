namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public static class TwitchGlobalBadgesData
    {
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
    }
}