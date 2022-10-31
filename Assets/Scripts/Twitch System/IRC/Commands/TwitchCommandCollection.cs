namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Collections.Generic;

    public class TwitchCommandCollection
    {
        private Dictionary<string, ITwitchCommandHandler> commands;

        public TwitchCommandCollection()
        {
            commands = new Dictionary<string, ITwitchCommandHandler>(); 
            commands.Add(TwitchStaticData.commandMessage, new TwitchCommandMessage());
            //TODO : Add here every command that you have created and you want to implement in the dictionnary, for example :
            // commands.Add(TwitchData.commandTest, new TwitchCommandTest());
            // commands.Add(TwitchData.commandJapon, new TwitchCommandJapon());
        }

        public bool HasCommand(string command)
        {
            return commands.ContainsKey(command) ? true : false;
        }

        public void ExecuteCommand(string command, TwitchStaticData.TwitchCommandData data)
        {
            command = command.Substring(1); //* To remove exclamation point.
            if (HasCommand(command)) //TODO : Add here badges and roles verifications with a dedicated function
            {
                commands[command].HandleCommmand(data);
            }
        }
    }
}