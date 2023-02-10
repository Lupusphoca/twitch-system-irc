namespace PierreARNAUDET.TwitchUtilitary
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;
    using PierreARNAUDET.Core.Events;
    using static PierreARNAUDET.TwitchUtilitary.TwitchGlobalEmotesData.GlobalEmotes;

    class TwitchFilterGlobalEmote : MonoBehaviour
    {
        [Events]
        [SerializeField] StringEvent idEvent;
        [SerializeField] StringEvent nameEvent;
        [SerializeField] StringEvent url1xEvent;
        [SerializeField] StringEvent url2xEvent;
        [SerializeField] StringEvent url4xEvent;
        [SerializeField] StringArrayEvent formatEvent;
        [SerializeField] StringArrayEvent scaleEvent;
        [SerializeField] StringArrayEvent themeModeEvent;

        public void Filter(GlobalEmote globalEmote)
        {
            idEvent.Invoke(globalEmote.Id);

        }
    }
}