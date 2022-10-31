namespace PierreARNAUDET.TwitchUtilitary
{
    public interface ITwitchCommandHandler
    {
        void HandleCommmand(TwitchStaticData.TwitchCommandData data);
    }
}