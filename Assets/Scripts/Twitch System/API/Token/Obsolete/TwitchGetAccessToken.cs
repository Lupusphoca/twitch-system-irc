// namespace PierreARNAUDET.TwitchUtilitary
// {
//     using System;
//     using System.Collections.Generic;
//     using System.Threading.Tasks;
//     using Newtonsoft.Json;

//     using UnityEngine;

//     using static PierreARNAUDET.TwitchUtilitary.Data.TwitchStaticData;

//     [Obsolete]
//     class TwitchGetAccessToken
//     {
//         public async Task GetUserAccessTokenFromCode(bool forceVerify)
//         {
//             var loadUserTokenData = PlayerPrefs.GetString(TwitchUtilitaryManager.userAccessTokenKey);  //* Return string.Empty if nothing saved
//             if (forceVerify || loadUserTokenData == string.Empty)
//             {
//                 var twitchGetAuthorizationCode = new TwitchGetAuthorizationCode();
//                 twitchGetAuthorizationCode.ExecuteAuthorizationCodeFlow();

//                 do
//                 {
//                     await Task.Yield();
//                 } while (twitchCredentials.AuthorizationCode == null || twitchCredentials.AuthorizationCode == string.Empty);

//                 await GetUserAccessToken();
//             }
//             else
//             {
//                 twitchCredentials.userAccessTokenData = JsonConvert.DeserializeObject<UserAccessTokenData>(loadUserTokenData);
//                 Debug.Log($"<color={HexColorCommonConsole}>Stored User Access Token</color> {twitchCredentials.userAccessTokenData.AccessToken}");
//             }
//         }

//         private async Task GetUserAccessToken()
//         {
//             var url = "https://id.twitch.tv/oauth2/token";

//             var formFields = new Dictionary<string, string> { };
//             formFields.Add("client_id", twitchCredentials.ClientId);
//             formFields.Add("client_secret", twitchCredentials.ClientSecret);
//             formFields.Add("code", twitchCredentials.AuthorizationCode);
//             formFields.Add("grant_type", "authorization_code");
//             formFields.Add("redirect_uri", twitchCredentials.RedirectURL);

//             var tuwr = new TwitchUnityWebRequest();
//             var result = await tuwr.Post(url, formFields);

//             twitchCredentials.userAccessTokenData = JsonConvert.DeserializeObject<UserAccessTokenData>(result);

//             PlayerPrefs.SetString(TwitchUtilitaryManager.userAccessTokenKey, result);
//             PlayerPrefs.Save();

//             Debug.Log($"<color={HexColorSuccess}>User Access Token</color> {twitchCredentials.userAccessTokenData.AccessToken}");
//         }

//         public async Task GetAppAccessToken(bool forceVerify)
//         {
//             var loadAppTokenData = PlayerPrefs.GetString(TwitchUtilitaryManager.appAccessTokenKey);  //* Return string.Empty if nothing saved
//             if (forceVerify || loadAppTokenData == string.Empty)
//             {
//                 var url = "https://id.twitch.tv/oauth2/token";

//                 var formFields = new Dictionary<string, string> { };
//                 formFields.Add("client_id", twitchCredentials.ClientId);
//                 formFields.Add("client_secret", twitchCredentials.ClientSecret);
//                 formFields.Add("grant_type", "client_credentials");

//                 var tuwr = new TwitchUnityWebRequest();
//                 var result = await tuwr.Post(url, formFields);

//                 twitchCredentials.appAccessTokenData = JsonConvert.DeserializeObject<AppAccessTokenData>(result);

//                 PlayerPrefs.SetString(TwitchUtilitaryManager.appAccessTokenKey, result);
//                 PlayerPrefs.Save();
//                 Debug.Log($"<color={HexColorSuccess}>App Access Token</color> {twitchCredentials.appAccessTokenData.AccessToken}");
//             }
//             else
//             {
//                 twitchCredentials.appAccessTokenData = JsonConvert.DeserializeObject<AppAccessTokenData>(loadAppTokenData);
//                 Debug.Log($"<color={HexColorCommonConsole}>Stored App Access Token</color> {twitchCredentials.appAccessTokenData.AccessToken}");
//             }
//         }
//     }
// }