namespace PierreARNAUDET.TwitchUtilitary
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using PierreARNAUDET.Core.Events;
    using static PierreARNAUDET.TwitchUtilitary.TwitchPubSubEventStruct;

    class TwitchPubSubFilterBitsEvent : MonoBehaviour
    {
        [Events]
        [SerializeField] ObjectEvent badgeEntitlementEvent;
        [SerializeField] IntEvent bitsUsedEvent;
        [SerializeField] StringEvent channelIdEvent;
        [SerializeField] StringEvent chatMessageEvent;
        [SerializeField] StringEvent contextMessageEvent;
        [SerializeField] BoolEvent isAnonymousEvent;
        [SerializeField] StringEvent messageIdEvent;
        [SerializeField] StringEvent timeEvent;
        [SerializeField] StringEvent totalBitsUsedEvent;
        [SerializeField] StringEvent userIdEvent;
        [SerializeField] StringEvent userNameEvent;
        [SerializeField] StringEvent versionEvent;

        public void Filter(PubSubBitsEventV2Message bitsEventV2Message)
        {
            badgeEntitlementEvent.Invoke(bitsEventV2Message.BadgeEntitlement);
            bitsUsedEvent.Invoke(bitsEventV2Message.BitsUsed);
            channelIdEvent.Invoke(bitsEventV2Message.ChannelId);
            chatMessageEvent.Invoke(bitsEventV2Message.ChatMessage);
            contextMessageEvent.Invoke(bitsEventV2Message.Context);
            isAnonymousEvent.Invoke(bitsEventV2Message.IsAnonymous);
            messageIdEvent.Invoke(bitsEventV2Message.MessageId);
            timeEvent.Invoke(bitsEventV2Message.Time);
            totalBitsUsedEvent.Invoke(bitsEventV2Message.TotalBitsUsed);
            userIdEvent.Invoke(bitsEventV2Message.UserId);
            userNameEvent.Invoke(bitsEventV2Message.UserName);
            versionEvent.Invoke(bitsEventV2Message.Version);
        }
    }
}