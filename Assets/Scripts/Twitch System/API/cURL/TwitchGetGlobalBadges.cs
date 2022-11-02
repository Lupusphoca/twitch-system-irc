namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using UnityEngine;

    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;
    using static PierreARNAUDET.TwitchUtilitary.TwitchGlobalBadgesData;

    class TwitchGetGlobalBadges
    {
        public async Task GetGlobalBadges()
        {
            var url = "https://api.twitch.tv/helix/chat/badges/global";

            var tuwr = new TwitchUnityWebRequest();
            var result = await tuwr.Get(url);

            globalBadges = JsonConvert.DeserializeObject<GlobalBadges>(result);
            Debug.Log($"{"Get Global Badges cURL".ColorString(ColorType.Success)} Success");
        }
    }
}