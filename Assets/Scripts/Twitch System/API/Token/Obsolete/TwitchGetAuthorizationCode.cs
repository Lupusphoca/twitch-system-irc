// namespace PierreARNAUDET.TwitchUtilitary
// {
//     using System;
//     using System.Net;

//     using UnityEngine;
//     using UnityEngine.Networking;

//     using static PierreARNAUDET.TwitchUtilitary.Data.TwitchStaticData;

//     [Obsolete]
//     class TwitchGetAuthorizationCode
//     {
//         private string safetyCode;

//         public void ExecuteAuthorizationCodeFlow()
//         {
//             safetyCode = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

//             for (int i = 0; i < twitchCredentials.Scopes.Length; i++)
//             {
//                 twitchCredentials.Scopes[i] = UnityWebRequest.EscapeURL(twitchCredentials.Scopes[i]);
//             }

//             var url =
//             "response_type=code&" +
//             "client_id=" + twitchCredentials.ClientId + "&" +
//             "redirect_uri=" + twitchCredentials.RedirectURL + "&" +
//             "scope=" + String.Join("+", twitchCredentials.Scopes) + "&" +
//             "state=" + safetyCode;

//             StartLocalWebServer();
//             Application.OpenURL("https://id.twitch.tv/oauth2/authorize" + "?" + url);
//         }

//         private void StartLocalWebServer()
//         {
//             var httpListener = new HttpListener();
//             httpListener.Prefixes.Add(twitchCredentials.RedirectURL);
//             httpListener.Start();
//             httpListener.BeginGetContext(new AsyncCallback(IncomingHttpRequest), httpListener);
//         }

//         private void IncomingHttpRequest(IAsyncResult asyncResult)
//         {
//             // Get back the reference to our http listener
//             var httpListener = (HttpListener)asyncResult.AsyncState;

//             // Fetch the context object
//             var httpContext = httpListener.EndGetContext(asyncResult);

//             // If we'd like the HTTP listener to accept more incoming requests, we'd just restart the "get context" here:
//             // httpListener.BeginGetContext(new AsyncCallback(IncomingHttpRequest), httpListener);
//             // However, since we only want/expect the one, single auth redirect, we don't need/want this, now
//             // But this is what you would do if you'd want to implement more (simple) "webserver" functionality in your project

//             // The context object has the request object for us, that holds details about the incoming request
//             var httpRequest = httpContext.Request;

//             twitchCredentials.AuthorizationCode = httpRequest.QueryString.Get("code");
//             Debug.Log($"<color={HexColorSuccess}>Authorization Code</color> {twitchCredentials.AuthorizationCode}");

//             // Build a response to send an "ok" back to the browser for the user to see
//             var httpResponse = httpContext.Response;
//             var responseString = "<html><body><b>DONE!</b><br>(You can close this tab/window now)</body></html>";
//             var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

//             // Send the output to the client browser
//             httpResponse.ContentLength64 = buffer.Length;
//             var output = httpResponse.OutputStream;
//             output.Write(buffer, 0, buffer.Length);
//             output.Close();

//             // The HTTP listener has served it's purpose, shut it down
//             // OnAuthorizationCodeFound?.Invoke();
//             httpListener.Stop();
//             // Obv. If we had restarted the waiting for more incoming request, above, we'd not Stop() it here.
//         }
//     }
// }