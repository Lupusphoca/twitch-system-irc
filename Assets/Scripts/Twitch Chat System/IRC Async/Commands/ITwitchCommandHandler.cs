namespace PierreARNAUDET.TwitchChat
{
    public interface ITwitchCommandHandler
    {
        void HandleCommmand(TwitchData.TwitchCommandData data);
    }
}