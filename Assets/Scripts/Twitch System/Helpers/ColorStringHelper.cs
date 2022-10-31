namespace PierreARNAUDET.TwitchUtilitary
{
    public static class ColorStringHelper
    {
        #region Debug Log
        public static string HexColorIRC { get; set; }
        public static string HexColorPubSubReceive { get; set; }
        public static string HexColorPubSubSend { get; set; }
        public static string HexColorSuccess { get; set; }
        public static string HexColorError { get; set; }
        public static string HexColorCommonConsole { get; set; }
        #endregion

        public enum ColorType
        {
            IRC,
            PubSubReceive,
            PubSubSend,
            Success,
            Error,
            CommonConsole
        }

        public static string ColorString(this string text, ColorType colorType)
        {
            var color = string.Empty;
            switch (colorType)
            {
                case ColorType.IRC:
                    color = HexColorIRC;
                    break;
                case ColorType.PubSubReceive:
                    color = HexColorPubSubReceive;
                    break;
                case ColorType.PubSubSend:
                    color = HexColorPubSubSend;
                    break;
                case ColorType.Success:
                    color = HexColorSuccess;
                    break;
                case ColorType.Error:
                    color = HexColorError;
                    break;
                case ColorType.CommonConsole:
                    color = HexColorCommonConsole;
                    break;
            }
            return $"<color={color}>{text}</color>";
        }
    }
}