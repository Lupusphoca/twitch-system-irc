namespace PierreARNAUDET.TwitchUtilitary.EventSub.Attributes
{
    using System;
    
    public class EventSubStatusAttribute : Attribute
    {
        public EventSubStatusAttribute(string status)
        {
            Status = status;
        }

        public string Status { get; }
    }
}