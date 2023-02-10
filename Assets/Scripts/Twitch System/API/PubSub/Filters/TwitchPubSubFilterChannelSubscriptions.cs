namespace PierreARNAUDET.TwitchUtilitary
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using PierreARNAUDET.Core.Events;
    using static PierreARNAUDET.TwitchUtilitary.TwitchPubSubEventStruct;

    class TwitchPubSubFilterChannelSubscriptions : MonoBehaviour
    {
        [Events]
        [SerializeField] StringEvent userNameEvent;
        [SerializeField] StringEvent displayNameEvent;
        [SerializeField] StringEvent channelNameEvent;
        [SerializeField] StringEvent userIdEvent;
        [SerializeField] StringEvent timeEvent;
        [SerializeField] StringEvent subPlanEvent;
        [SerializeField] StringEvent subPlanNameEvent;
        [SerializeField] IntEvent monthsEvent;
        [SerializeField] IntEvent multiMonthsDurationEvent;
        [SerializeField] IntEvent cumulativeMonthsEvent;
        [SerializeField] IntEvent streakMonthsEvent;
        [SerializeField] StringEvent contextEvent;
        [SerializeField] BoolEvent isGiftEvent;

        [SerializeField] StringEvent subMessageMessageEvent;
        [SerializeField] IntEvent subMessageEmoteStartEvent;
        [SerializeField] IntEvent subMessageEmoteEndEvent;
        [SerializeField] IntEvent subMessageEmoteIdEvent;

        [SerializeField] StringEvent recipientIdEvent;
        [SerializeField] StringEvent reicipientUserNameEvent;
        [SerializeField] StringEvent recipientDisplayNameEvent;

        public void Filter(PubSubChannelSubscriptionsMessage channelSubscriptionsMessage)
        {
            userNameEvent.Invoke(channelSubscriptionsMessage.UserName);
            displayNameEvent.Invoke(channelSubscriptionsMessage.DisplayName);
            channelNameEvent.Invoke(channelSubscriptionsMessage.ChannelName);
            userIdEvent.Invoke(channelSubscriptionsMessage.UserId);
            timeEvent.Invoke(channelSubscriptionsMessage.Time);
            subPlanEvent.Invoke(channelSubscriptionsMessage.SubPlan);
            subPlanNameEvent.Invoke(channelSubscriptionsMessage.SubPlanName);
            monthsEvent.Invoke(channelSubscriptionsMessage.Months);
            multiMonthsDurationEvent.Invoke(channelSubscriptionsMessage.MultiMonthDuration);
            cumulativeMonthsEvent.Invoke(channelSubscriptionsMessage.CumulativeMonths);
            streakMonthsEvent.Invoke(channelSubscriptionsMessage.StreakMonths);
            contextEvent.Invoke(channelSubscriptionsMessage.Context);
            isGiftEvent.Invoke(channelSubscriptionsMessage.IsGift);
            subMessageMessageEvent.Invoke(channelSubscriptionsMessage.UserName);

            for (int i = 0; i < channelSubscriptionsMessage._SubMessage.Emotes.Count; i++)
            {
                subMessageEmoteStartEvent.Invoke(channelSubscriptionsMessage._SubMessage.Emotes[i].Start);
                subMessageEmoteEndEvent.Invoke(channelSubscriptionsMessage._SubMessage.Emotes[i].End);
                subMessageEmoteIdEvent.Invoke(channelSubscriptionsMessage._SubMessage.Emotes[i].Id);
            }

            recipientIdEvent.Invoke(channelSubscriptionsMessage.RecipientId);
            reicipientUserNameEvent.Invoke(channelSubscriptionsMessage.RecipientUserName);
            recipientDisplayNameEvent.Invoke(channelSubscriptionsMessage.RecipientDisplayName);
        }
    }
}