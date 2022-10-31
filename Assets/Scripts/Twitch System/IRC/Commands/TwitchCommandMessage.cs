namespace PierreARNAUDET.TwitchUtilitary
{
    using UnityEngine;
    using static PierreARNAUDET.TwitchUtilitary.TwitchStaticData;
    
    public class TwitchCommandMessage : ITwitchCommandHandler
    {
        //EXAMPLES - This is how I would impletement this interface and create classes with actual command logic

        //* !message command
        public void HandleCommmand(TwitchCommandData data) 
        {
            Debug.Log($"<color=cyan>Raw Message:{data.Message}</color>");

            // strip the !message command from the message and trim the leading whitespace
            string actualMessage = data.Message.Substring(0 + (commandPrefix + commandMessage).Length).TrimStart(' ');
            Debug.Log($"<color=cyan>{data.Author} says {actualMessage}</color>");
        }
    }
}