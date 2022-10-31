namespace PierreARNAUDET.TwitchUtilitary.EventSub.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using PierreARNAUDET.TwitchUtilitary.EventSub.Models;
    using PierreARNAUDET.TwitchUtilitary.EventSub.Models.Responses;

    public interface IEventSub
    {
        Task<TwitchEventSubs> GetEventsAsync();

        // Task<CreateSubscription> CreateStreamOnlineEventAsync(string channelId, Uri webHookUrl);

        Task<CreateSubscription> CreateStreamOfflineEventAsync(string channelId, Uri webHookUrl);

        Task DeleteEventAsync(string subscriptionId);
    }
}