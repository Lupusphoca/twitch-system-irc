namespace PierreARNAUDET.TwitchUtilitary
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEngine.Networking;

    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;

    class TwitchOAuthImplicitCodeFlow
    {
        private string safetyCode;

        public void ExecuteFlow()
        {
            safetyCode = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

            for (int i = 0; i < twitchCredentials.Scopes.Length; i++)
            {
                twitchCredentials.Scopes[i] = UnityWebRequest.EscapeURL(twitchCredentials.Scopes[i]);
            }

            var url =
            "client_id=" + twitchCredentials.ClientId + "&" +
            "redirect_uri=" + twitchCredentials.RedirectURL + "&" +
            "response_type=token&" +
            "scope=" + String.Join("+", twitchCredentials.Scopes) + "&" +
            "state=" + safetyCode;

            StartLocalWebServer();
            Application.OpenURL("https://id.twitch.tv/oauth2/authorize" + "?" + url);
        }

        private void StartLocalWebServer()
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add(twitchCredentials.RedirectURL);
            httpListener.Start();
            httpListener.BeginGetContext(new AsyncCallback(IncomingHttpRequest), httpListener);
        }

        private void IncomingHttpRequest(IAsyncResult asyncResult)
        {
            // Get back the reference to our http listener
            var httpListener = (HttpListener)asyncResult.AsyncState;

            // Fetch the context object
            var httpContext = httpListener.EndGetContext(asyncResult);

            httpListener.BeginGetContext(new AsyncCallback(IncomingAuth), httpListener);

            // The context object has the request object for us, that holds details about the incoming request
            var httpRequest = httpContext.Request;

            // Build a response to send an "ok" back to the browser for the user to see
            var httpResponse = httpContext.Response;

            //* Code needed to execute the next part of code
            var responseString = "<html><body><b id=\"auth\">DONE!</b><br>(You can close this tab/window now)<br>" +
            "<script type=\"text/javascript\">" +
            "var xhr = new XMLHttpRequest(); " +
            $"xhr.open(\"POST\", \"{UnityWebRequest.EscapeURL(twitchCredentials.RedirectURL)}\");" +
            "xhr.send(window.location);" + //Sending the window location (the url bar) from the browser to our listener
            "</script></body></html>";

            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            // Send the output to the client browser
            httpResponse.ContentLength64 = buffer.Length;
            var output = httpResponse.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        private void IncomingAuth(IAsyncResult asyncResult)
        {
            var httpListener = (HttpListener)asyncResult.AsyncState;

            httpListener = (HttpListener)asyncResult.AsyncState;

            var httpContext = httpListener.EndGetContext(asyncResult);

            httpListener.BeginGetContext(new AsyncCallback(IncomingAuth), httpListener);

            var httpRequest = httpContext.Request;

            // This time we take an input stream from the request to recieve the url
            var reader = new StreamReader(httpRequest.InputStream, httpRequest.ContentEncoding);
            var url = reader.ReadToEnd();

            // Regex to extract the OAUTH and auth state
            var rx = new Regex(@".+#access_token=(.+)&scope.*state=(\d+)");
            var match = rx.Match(url);

            // If state doesnt match reject data
            if (match.Groups[2].Value != safetyCode)
            {
                return;
            }

            accessToken = match.Groups[1].Value;
            
            var debug = $"{"OAuth Access Token".ColorString(ColorType.Success)} : Success";
            Debug.Log(debug);
            twitchInformationBox.Display(debug);

            httpListener.Stop();
        }
    }
}