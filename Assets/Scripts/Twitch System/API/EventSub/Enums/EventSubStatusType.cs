namespace PierreARNAUDET.TwitchUtilitary.EventSub.Enums
{
    using PierreARNAUDET.TwitchUtilitary.EventSub.Attributes;

    public enum EventSubStatusType
    {
        None,
        [EventSubStatus("enabled")] Enabled,
        [EventSubStatus("webhook_callback_verification_pending")] Pending,
        [EventSubStatus("webhook_callback_verification_failed")] Failed,
        [EventSubStatus("notification_failures_exceeded")] FailuresExceeded,
        [EventSubStatus("authorization_revoked")] Revoked,
        [EventSubStatus("user_removed")] Removed
    }
}