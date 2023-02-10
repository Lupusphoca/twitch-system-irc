namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    using UnityEngine;

    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;
    using static PierreARNAUDET.TwitchUtilitary.TwitchGlobalEmotesData;

    class TwitchGetGlobalEmotes
    {
        public async Task GetGlobalEmotes()
        {
            var url = "https://api.twitch.tv/helix/chat/emotes/global";

            var tuwr = new TwitchUnityWebRequest();
            var result = await tuwr.Get(url);

            globalEmotes = JsonConvert.DeserializeObject<GlobalEmotes>(result);

            var debug = $"{"Get Global Emotes cURL".ColorString(ColorType.Success)} : Success";
            Debug.Log(debug);
            twitchInformationBox.Display(debug);
        }
    }
}