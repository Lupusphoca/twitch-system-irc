namespace PierreARNAUDET.TwitchUtilitary
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using PierreARNAUDET.Core.Events;
    using static PierreARNAUDET.TwitchUtilitary.TwitchPubSubEventStruct;

    class TwitchPubSubFilterChannelPoints : MonoBehaviour
    {
        [Events]
        [SerializeField] StringEvent timestampEvent;
        [SerializeField] StringEvent redemptionChannelIdEvent;
        [SerializeField] StringEvent redemptionRedeemedAtEvent;
        [SerializeField] ObjectEvent redemptionRewardEvent;
        [SerializeField] StringEvent redemptionUserInputEvent;
        [SerializeField] StringEvent redemptionStatusEvent;

        public void Filter(PubSubChannelPointsMessage channelPointsMessage)
        {
            timestampEvent.Invoke(channelPointsMessage.Timestamp);
            redemptionChannelIdEvent.Invoke(channelPointsMessage._Redemption.ChannelId);
            redemptionRedeemedAtEvent.Invoke(channelPointsMessage._Redemption.RedeemedAt);
            redemptionRewardEvent.Invoke(channelPointsMessage._Redemption.Reward);
            redemptionUserInputEvent.Invoke(channelPointsMessage._Redemption.UserInput);
            redemptionStatusEvent.Invoke(channelPointsMessage._Redemption.Status);
        }
    }
}