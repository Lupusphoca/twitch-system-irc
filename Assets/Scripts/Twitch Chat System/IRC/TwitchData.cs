namespace PierreARNAUDET.TwitchChat
{
    using System.IO;
    using System.Net.Sockets;

    public static class TwitchData
    {
        #region Connection
        public struct TwitchCredentials
        {
            public string ChannelName;
            public string Username;
            public string Password;
        }

        public static TwitchCredentials twitchCredentials;
        #endregion

        #region Commands
        public static readonly string commandPrefix = "!";
        public static readonly string commandMessage = "message";

        public struct TwitchCommandData
        {
            public string Author;
            public string Message;
        }
        #endregion

        public static TwitchCommandCollection commandCollection;

        #region Network
        public static TcpClient tcpClient;
        public static StreamReader streamReader;
        public static StreamWriter streamWriter;
        #endregion

        public static string[] messages = new string[20];
    }
}