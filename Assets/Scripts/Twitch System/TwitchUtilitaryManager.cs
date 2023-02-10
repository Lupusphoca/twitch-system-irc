namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;

    public class TwitchUtilitaryManager : MonoBehaviour
    {
        [field: SerializeField] public Color colorCommonConsole { get; private set; }
        [field: SerializeField] public Color colorIRC { get; private set; }
        [field: SerializeField] public Color colorPubSubReceive { get; private set; }
        [field: SerializeField] public Color colorPubSubSend { get; private set; }
        [field: SerializeField] public Color colorSuccess { get; private set; }
        [field: SerializeField] public Color colorError { get; private set; }

        [Data]
        [SerializeField] private TwitchInformationBox twitchInformationBox;

        [Settings]
        [SerializeField, Range(10, 100)] private int displayMessages = 20;
        [SerializeField, Range(10, 100)] private int displayInformations = 10;

        public const string userAccessTokenKey = "user_access_token_key";
        public const string appAccessTokenKey = "app_access_token_key";
        public const string authorizationCodeKey = "authorization_code_key";

        public static event Action OnAccessTokenAuthorized;

        private void Start()
        {
            var path = Path.Combine(UnityEngine.Application.streamingAssetsPath, "credentials.json");
            var jsonText = File.ReadAllText(path);
            twitchCredentials = JsonConvert.DeserializeObject<TwitchCredentials>(jsonText);

            ExceptionHelper.ThrowIfEmpty(twitchCredentials.ClientId, string.IsNullOrWhiteSpace, $"The client ID is required, create a Twitch app here {ExceptionHelper.APP_REGISTRATION}");
            ExceptionHelper.ThrowIfEmpty(twitchCredentials.RedirectURL, string.IsNullOrWhiteSpace, $"The redirect url is required, create a Twitch app here {ExceptionHelper.APP_REGISTRATION}");
            ExceptionHelper.ThrowIfEmpty(twitchCredentials.Scopes, value => value.Length == 0, $"At least one scope is required");

            messages = new string[displayMessages];
            TwitchStaticData.twitchInformationBox = twitchInformationBox;
            informations = new string[displayInformations];

            HexColorIRC = "#" + ColorUtility.ToHtmlStringRGB(colorIRC);
            HexColorPubSubReceive = "#" + ColorUtility.ToHtmlStringRGB(colorPubSubReceive);
            HexColorPubSubSend = "#" + ColorUtility.ToHtmlStringRGB(colorPubSubSend);
            HexColorSuccess = "#" + ColorUtility.ToHtmlStringRGB(colorSuccess);
            HexColorError = "#" + ColorUtility.ToHtmlStringRGB(colorError);
            HexColorCommonConsole = "#" + ColorUtility.ToHtmlStringRGB(colorCommonConsole);

            _ = ExecuteAsync(); // Same as GetAppAccessToken().Start();
        }

        private async Task ExecuteAsync()
        {
            var twitchOAuthImplicitCodeFlow = new TwitchOAuthImplicitCodeFlow();
            twitchOAuthImplicitCodeFlow.ExecuteFlow();

            do
            {
                await Task.Yield();
            } while (accessToken == null || accessToken == string.Empty);


            var twitchGetUsers = new TwitchGetUsers();
            await twitchGetUsers.GetUsers();

            var twitchGetGlobalBadges = new TwitchGetGlobalBadges();
            await twitchGetGlobalBadges.GetGlobalBadges();

            var twitchGetGlobalEmotes = new TwitchGetGlobalEmotes();
            await twitchGetGlobalEmotes.GetGlobalEmotes();

            OnAccessTokenAuthorized?.Invoke();
        }
    }
}