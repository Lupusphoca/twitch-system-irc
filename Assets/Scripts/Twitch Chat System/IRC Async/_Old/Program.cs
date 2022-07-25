namespace PierreARNAUDET.TwitchChat
{
    using System;
    using System.Threading.Tasks;

    using UnityEngine;

    using AsyncAwaitBestPractices;

    using PierreARNAUDET.Core.Attributes;

    class Program : MonoBehaviour
    {
        [Data]
        [SerializeField] string password;
        [SerializeField] string botUsername;
        [SerializeField] string twitchChannelName;
        [SerializeField] Main twitchBot;

        async Task Start()
        {
            await Main();
        }

        async Task Main()
        {
            await twitchBot.Run();
            //We could .SafeFireAndForget() these two calls if we want to
            // await twitchBot.JoinChannel(twitchChannelName);
            // await twitchBot.SendMessage(twitchChannelName, "Hey my bot has started up !");

            // twitchBot.OnMessage += async (sender, twitchChatMessage) =>
            // {
            //     Console.WriteLine($"{twitchChatMessage.Sender} said '{twitchChatMessage.Message}'");
            //     //Listen for !hey command
            //     if (twitchChatMessage.Message.StartsWith("!hey"))
            //     {
            //         await twitchBot.SendMessage(twitchChatMessage.Channel, $"Hey there {twitchChatMessage.Sender}");
            //     }
            // };
            //* register command / event in OnMessage, when this event is trigger in Bot call the program above to check the !hey process

            // Debug.Log("Test");

            // await Task.Delay(-1);
        }
    }
}