namespace PierreARNAUDET.TwitchUtilitary.EventSub.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.All)]
    public class EventSubTypeAttribute : Attribute
    {
        public EventSubTypeAttribute(string type, string scope = default)
        {
            Type = type;
            Scope = scope;
        }

        public string Type { get; }

        public string Scope { get; }
    }
}