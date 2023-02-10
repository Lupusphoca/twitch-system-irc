namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    using UnityEngine;

    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;

    class TwitchGetUsers
    {
        public async Task GetUsers()
        {
            var url = $"https://api.twitch.tv/helix/users?login={twitchCredentials.ChannelConnection}";

            var tuwr = new TwitchUnityWebRequest();
            var result = await tuwr.Get(url);

            twitchUser = JsonConvert.DeserializeObject<TwitchUser>(result);

            var debug = $"{"Get Users cURL".ColorString(ColorType.Success)} : Success";
            Debug.Log(debug);
            twitchInformationBox.Display(debug);
        }
    }
}