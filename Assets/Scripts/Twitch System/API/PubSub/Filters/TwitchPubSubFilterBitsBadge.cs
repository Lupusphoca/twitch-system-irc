namespace PierreARNAUDET.TwitchUtilitary
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using PierreARNAUDET.Core.Events;
    using static PierreARNAUDET.TwitchUtilitary.TwitchPubSubEventStruct;

    class TwitchPubSubFilterBitsBadge : MonoBehaviour
    {
        [Events]
        [SerializeField] StringEvent userIdEvent;
        [SerializeField] StringEvent userNameEvent;
        [SerializeField] StringEvent channelIdEvent;
        [SerializeField] StringEvent channelNameEvent;
        [SerializeField] IntEvent badgeTierEvent;
        [SerializeField] StringEvent chatMessageEvent;
        [SerializeField] StringEvent timeEvent;

        public void Filter(PubSubBitsBadgeMessage bitsBadgeMessage)
        {
            userIdEvent.Invoke(bitsBadgeMessage.UserId);
            userNameEvent.Invoke(bitsBadgeMessage.UserName);
            channelIdEvent.Invoke(bitsBadgeMessage.ChannelId);
            channelNameEvent.Invoke(bitsBadgeMessage.ChannelName);
            badgeTierEvent.Invoke(bitsBadgeMessage.BadgeTier);
            chatMessageEvent.Invoke(bitsBadgeMessage.ChatMessage);
            timeEvent.Invoke(bitsBadgeMessage.Time);
        }
    }
}