namespace PierreARNAUDET.TwitchUtilitary
{
    using System;

    public static class ExceptionHelper
    {
        public const string APP_REGISTRATION = "https://dev.twitch.tv/docs/authentication#registration";

        public static void ThrowIfEmpty<T>(T value, Func<T, bool> pred, string message)
        {
            if (pred(value))
            {
                throw new ArgumentException(message);
            }
        }

    }
}
