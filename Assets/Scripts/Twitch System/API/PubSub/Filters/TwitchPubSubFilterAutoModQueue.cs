namespace PierreARNAUDET.TwitchUtilitary
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using PierreARNAUDET.Core.Events;
    using static PierreARNAUDET.TwitchUtilitary.TwitchPubSubEventStruct;

    class TwitchPubSubFilterAutoModQueue : MonoBehaviour
    {
        [Events]
        [SerializeField] StringEvent idEvent;
        [SerializeField] StringEvent contentTextEvent;
        [SerializeField] ObjectListEvent contentFragmentsEvent;
        [SerializeField] IntEvent senderUserIdEvent;
        [SerializeField] StringEvent senderLoginEvent;
        [SerializeField] StringEvent senderDisplayNameEvent;
        [SerializeField] StringEvent senderChatColorEvent;
        [SerializeField] StringEvent sentAtEvent;

        [SerializeField] StringEvent categoryEvent;
        [SerializeField] IntEvent levelEvent;

        [SerializeField] StringEvent reasonCodeEvent;
        [SerializeField] StringEvent resolverIdEvent;
        [SerializeField] StringEvent resolverLoginEvent;

        public void FilterMessage(PubSubAutoModQueueMessage autoModQueueMessage)
        {
            idEvent.Invoke(autoModQueueMessage.Id);
            contentTextEvent.Invoke(autoModQueueMessage._Content.Text);
            contentFragmentsEvent.Invoke(autoModQueueMessage._Content._Fragments);
            senderUserIdEvent.Invoke(autoModQueueMessage._Sender.UserId);
            senderLoginEvent.Invoke(autoModQueueMessage._Sender.Login);
            senderDisplayNameEvent.Invoke(autoModQueueMessage._Sender.DisplayName);
            senderChatColorEvent.Invoke(autoModQueueMessage._Sender.ChatColor);
            sentAtEvent.Invoke(autoModQueueMessage.SentAt);
        }

        public void FilterContentClassification(PubSubAutoModQueueContentClassification autoModQueueContentClassification)
        {
            categoryEvent.Invoke(autoModQueueContentClassification.Category);
            levelEvent.Invoke(autoModQueueContentClassification.Level);
        }

        public void FilterStatus(PubSubAutoModQueueStatus autoModQueueStatus)
        {
            reasonCodeEvent.Invoke(autoModQueueStatus.ReasonCode);
            resolverIdEvent.Invoke(autoModQueueStatus.ResolverId);
            resolverLoginEvent.Invoke(autoModQueueStatus.ResolverLogin);
        }
    }
}