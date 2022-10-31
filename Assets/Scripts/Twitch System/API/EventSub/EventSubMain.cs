// namespace PierreARNAUDET.TwitchUtilitary.EventSub
// {
//     using System;
//     using System.Linq;
//     using System.Threading.Tasks;

//     using UnityEngine;

//     using PierreARNAUDET.TwitchUtilitary;
//     using static PierreARNAUDET.TwitchUtilitary.Data.TwitchStaticData;

//     public class EventSubMain : MonoBehaviour
//     {
//         private void OnEnable()
//         {
//             TwitchUtilitaryManager.OnAccessTokenAuthorized -= StartEventSubscription;
//             TwitchUtilitaryManager.OnAccessTokenAuthorized += StartEventSubscription;
//         }

//         private void StartEventSubscription()
//         {
//             _ = ExecuteAsync();
//         }

//         private async Task ExecuteAsync()
//         {
//             var eventSubService = new EventSubService();
//             var events = await eventSubService.GetEventsAsync();

//             if (!events.Data.Any()) //* If there is already event sub services
//             {
//                 // var streamOnline = await eventSubService.CreateStreamOnlineEventAsync(twitchCredentials.ClientId, new Uri("https://webhook.site/4fb7a208-b717-4c7f-aea7-a1768d8c0896")); //
//                 var streamOnline = await eventSubService.CreateStreamOnlineEventAsync();
//             }

//             // foreach (var twitchEventSub in events.Data)
//             // {
//             //     if (twitchEventSub.Status != "enabled" && twitchEventSub.Status != "webhook_callback_verification_pending")
//             //     {
//             //         await eventSubService.DeleteEventAsync(events.Data.First().Id);
//             //     }
//             //     Debug.Log($"[{twitchEventSub.Id}]: {twitchEventSub.Type} ({twitchEventSub.Status})");
//             // }
//         }
//     }
// }