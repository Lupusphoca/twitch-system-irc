namespace PierreARNAUDET.TwitchUtilitary
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using PierreARNAUDET.Core.Events;
    using static PierreARNAUDET.TwitchUtilitary.TwitchPubSubEventStruct;

    class TwitchPubSubFilterUserModerationNotification : MonoBehaviour
    {
        [Events]
        [SerializeField] StringEvent idEvent;
        [SerializeField] StringEvent statusEvent;

        public void Filter(PubSubUserModerationNotificationMessage userModerationNotificationMessage)
        {
            idEvent.Invoke(userModerationNotificationMessage.Id);
            statusEvent.Invoke(userModerationNotificationMessage.Status);
        }
    }
}