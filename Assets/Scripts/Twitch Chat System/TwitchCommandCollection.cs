namespace PierreARNAUDET.TwitchChat
{
    using System.Collections.Generic;

    public class TwitchCommandCollection
    {
        private Dictionary<string, ITwitchCommandHandler> commands;

        public TwitchCommandCollection()
        {
            commands = new Dictionary<string, ITwitchCommandHandler>();
            commands.Add(TwitchData.commandMessage, new TwitchCommandMessage());
        }

        public bool HasCommand(string command)
        {
            return commands.ContainsKey(command) ? true : false;
        }

        public void ExecuteCommand(string command, TwitchData.TwitchCommandData data)
        {
            command = command.Substring(1); //* To remove exclamation point.
            if (HasCommand(command))
            {
                commands[command].HandleCommmand(data);
            }
        }
    }
}