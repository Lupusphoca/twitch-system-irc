// namespace PierreARNAUDET.TwitchUtilitary.EventSub
// {
//     using System;
//     using System.Net.Http;
//     using System.Threading;
//     using System.Threading.Tasks;
//     using Newtonsoft.Json;

//     using UnityEngine;
//     using UnityEngine.Networking;

//     using PierreARNAUDET.TwitchUtilitary.EventSub.Interfaces;
//     using PierreARNAUDET.TwitchUtilitary.EventSub.Models;
//     using PierreARNAUDET.TwitchUtilitary.EventSub.Models.Responses;
//     using PierreARNAUDET.TwitchUtilitary.EventSub.Enums;
//     using PierreARNAUDET.TwitchUtilitary.EventSub.Attributes;
//     using PierreARNAUDET.TwitchUtilitary.EventSub.Extensions;
//     using static PierreARNAUDET.TwitchUtilitary.Data.TwitchStaticData;
//     using PierreARNAUDET.TwitchUtilitary.Helpers;

//     [Obsolete]
//     public class EventSubService : IEventSub
//     {
//         private int maxRetries = 2;
//         private int authRetryCounter;
//         private int retryCounter;
//         private TwitchClientToken twitchClientToken;

//         public async Task<TwitchEventSubs> GetEventsAsync()
//         {
//             var cancellationTokenSource = new CancellationTokenSource();
//             var httpClient = GenerateEventSubHttpClient(string.Empty); //! GenerateEventSubHttpClient(twitchCredentials.userTokenResponse.access_token);
//             if (httpClient == default)
//             {
//                 return default;
//             }

//             var twitchEventSubs = new TwitchEventSubs();

//             while (!cancellationTokenSource.IsCancellationRequested)
//             {
//                 var response = await httpClient.GetAsync("subscriptions", cancellationTokenSource.Token);
//                 twitchEventSubs = await ParseHttpReponse.ParseResponse<TwitchEventSubs>(response, cancellationTokenSource, maxRetries);
//             }

//             return twitchEventSubs;
//         }

//         public async Task<CreateSubscription> CreateStreamOnlineEventAsync(string channelId, Uri webHookUrl)
//         {
//             var cancellationTokenSource = new CancellationTokenSource();
//             var httpClient = GenerateEventSubHttpClient(twitchCredentials.appAccessTokenData.AccessToken);
//             if (httpClient == default)
//             {
//                 return default;
//             }

//             var eventType = EventSubType.ChannelFollow;
//             var eventData = eventType.GetAttributeOfType<EventSubTypeAttribute>();

//             var secret = Guid.NewGuid();
//             var streamOnlineEvent = new TwitchEventSub
//             {
//                 Type = eventData.Type,
//                 Version = "1",
//                 Condition = new Models.Condition
//                 {
//                     BroadcasterUserId = channelId
//                 },
//                 Transport = new Models.Transport
//                 {
//                     Method = "webhook",
//                     Callback = webHookUrl,
//                     Secret = secret.ToString("N")
//                 }
//             };

//             var streamOnline = new CreateSubscription();
//             while (!cancellationTokenSource.IsCancellationRequested)
//             {
//                 var response = await httpClient.PostAsNewtonsoftJsonAsync(new Uri("https://api.twitch.tv/helix/eventsub/subscriptions"), streamOnlineEvent, cancellationTokenSource.Token);
//                 streamOnline = await ParseHttpReponse.ParseResponse<CreateSubscription>(response, cancellationTokenSource, maxRetries);
//             }
//             return streamOnline;
//         }

//         public async Task<CreateSubscription> CreateStreamOnlineEventAsync()
//         {
//             var streamOnline = new CreateSubscription();
//             var cancellationTokenSource = new CancellationTokenSource();
//             var url = "https://api.twitch.tv/helix/eventsub/subscriptions";
//             var callbackUrl = "https://twitch-api-handler.firebaseio.com//eventsub/4FhfddT3LnY2k59tC3jH";
//             var eventType = EventSubType.StreamOnline;
//             var eventData = eventType.GetAttributeOfType<EventSubTypeAttribute>();

//             var secret = Guid.NewGuid();
//             var streamOnlineEvent = new TwitchEventSub
//             {
//                 Type = eventData.Type,
//                 Version = "1",
//                 Condition = new Models.Condition
//                 {
//                     BroadcasterUserId = twitchCredentials.ClientId,
//                 },
//                 Transport = new Models.Transport
//                 {
//                     Method = "webhook",
//                     Callback = new Uri(callbackUrl),
//                     Secret = secret.ToString("N")
//                 }
//             };
//             var content = JsonConvert.SerializeObject(streamOnlineEvent, Formatting.None);
//             Debug.Log($"Sended content for the EventSub : {content}");

//             var webRequest = UnityWebRequest.Post(url, content);
//             webRequest.SetRequestHeader("Authorization", $"Bearer {twitchCredentials.appAccessTokenData}");
//             webRequest.SetRequestHeader("Client-Id", twitchCredentials.ClientId);
//             webRequest.SetRequestHeader("Content-Type", "application/json");
//             webRequest.SendWebRequest();

//             while (!webRequest.isDone)
//             {
//                 await Task.Yield();
//             }

//             switch (webRequest.result)
//             {
//                 case UnityWebRequest.Result.ConnectionError:
//                     Debug.LogError("<color=red>Connection</color> : " + webRequest.error);
//                     break;
//                 case UnityWebRequest.Result.DataProcessingError:
//                     Debug.LogError("<color=red>Data Processing Error</color> : " + webRequest.error);
//                     break;
//                 case UnityWebRequest.Result.ProtocolError:
//                     Debug.LogError("<color=red>Protocol Error</color> : " + webRequest.error);
//                     break;
//                 case UnityWebRequest.Result.Success:
//                     Debug.Log($"Well done it was so easy : {webRequest.result}");
//                     break;
//             }
//             return streamOnline;
//         }

//         public Task<CreateSubscription> CreateStreamOfflineEventAsync(string channelId, Uri webHookUrl)
//         {
//             throw new NotImplementedException();
//         }

//         public async Task DeleteEventAsync(string subscriptionId)
//         {
//             throw new NotImplementedException();

//             // var cancellationTokenSource = new CancellationTokenSource();
//             // var httpClient = GenerateEventSubHttpClient();
//             // if (httpClient == default)
//             // {
//             //     return;
//             // }

//             // var queryBuilder = new QueryBuilder
//             // {
//             //     {"id", subscriptionId}
//             // };

//             // var uriBuilder = new UriBuilder(Shared.TwitchEventSubBaseUri) { Query = queryBuilder.ToString() };

//             // var response = await httpClient.DeleteAsync(uriBuilder.ToString(), cancellationTokenSource.Token);

//             // if (response.IsSuccessStatusCode)
//             // {
//             //     Debug.Log($"Successfully deleted event: {subscriptionId}");
//             // }
//             // else
//             // {
//             //     var responseString = await response.Content.ReadAsStringAsync(cancellationTokenSource.Token);
//             //     Debug.Log($"Failed to delete {subscriptionId}: [{response.StatusCode}] {responseString}");
//             // }
//         }

//         private HttpClient GenerateEventSubHttpClient(string token)
//         {
//             if (token != null)
//             {
//                 var httpClient = new HttpClient { BaseAddress = new Uri("https://api.twitch.tv/helix/eventsub/subscriptions") }; //* Common URL adress is used for Event Subscriptions
//                 httpClient.DefaultRequestHeaders.Clear();
//                 httpClient.DefaultRequestHeaders.Add("Client-ID", twitchCredentials.ClientId);
//                 httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
//                 return httpClient;
//             }
//             else
//             {
//                 return default;
//             }
//         }

//         // public async Task<CreateSubscription> CreateStreamOfflineEventAsync(string channelId, Uri webHookUrl)
//         // {
//         //     var cancellationTokenSource = new CancellationTokenSource();
//         //     var httpClient = await GenerateEventSubHttpClient();
//         //     if (httpClient == default) return default;

//         //     const EventSubType eventType = EventSubType.StreamOffline;
//         //     var eventData = eventType.GetAttributeOfType<EventSubTypeAttribute>();

//         //     var secret = Guid.NewGuid();

//         //     var streamOnlineEvent = new TwitchEventSub
//         //     {
//         //         Type = eventData.Type,
//         //         Version = "1",
//         //         Condition = new Condition
//         //         {
//         //             BroadcasterUserId = channelId
//         //         },
//         //         Transport = new Transport
//         //         {
//         //             Method = "webhook",
//         //             Callback = webHookUrl,
//         //             Secret = secret.ToString("N")
//         //         }
//         //     };

//         //     CreateSubscription streamOnline = null;

//         //     while (!cancellationTokenSource.IsCancellationRequested)
//         //     {
//         //         var response = await httpClient.PostAsNewtonsoftJsonAsync(Shared.TwitchEventSubSubscriptionsEndpoint,
//         //             streamOnlineEvent, cancellationTokenSource.Token);
//         //         streamOnline = await ParseResponse<CreateSubscription>(response, cancellationTokenSource);
//         //     }

//         //     return streamOnline;
//         // }
//     }
// }