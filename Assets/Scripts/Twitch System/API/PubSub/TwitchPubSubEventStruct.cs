namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    static class TwitchPubSubEventStruct
    {
        #region Bits Event V2 - Only Message Data
        public struct PubSubBitsEventV2Message
        {
            [JsonProperty("badge_entitlement", NullValueHandling = NullValueHandling.Ignore)]
            public object BadgeEntitlement { get; set; }
            [JsonProperty("bits_used", NullValueHandling = NullValueHandling.Ignore)]
            public int BitsUsed { get; set; }
            [JsonProperty("channel_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ChannelId { get; set; }
            [JsonProperty("chat_message", NullValueHandling = NullValueHandling.Ignore)]
            public string ChatMessage { get; set; }
            [JsonProperty("context", NullValueHandling = NullValueHandling.Ignore)]
            public string Context { get; set; }
            [JsonProperty("is_anonymous", NullValueHandling = NullValueHandling.Ignore)]
            public bool IsAnonymous { get; set; }
            [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageId { get; set; }
            [JsonProperty("message_type", NullValueHandling = NullValueHandling.Ignore)]
            public string MessageType { get; set; }
            [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
            public string Time { get; set; }
            [JsonProperty("total_bits_used", NullValueHandling = NullValueHandling.Ignore)]
            public string TotalBitsUsed { get; set; }
            [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
            public string UserId { get; set; }
            [JsonProperty("user_name", NullValueHandling = NullValueHandling.Ignore)]
            public string UserName { get; set; }
            [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
            public string Version { get; set; }
        }
        #endregion

        #region Bits Badge - Only Message Data
        public struct PubSubBitsBadgeMessage
        {
            [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
            public string UserId { get; set; }
            [JsonProperty("user_name", NullValueHandling = NullValueHandling.Ignore)]
            public string UserName { get; set; }
            [JsonProperty("channel_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ChannelId { get; set; }
            [JsonProperty("channel_name", NullValueHandling = NullValueHandling.Ignore)]
            public string ChannelName { get; set; }
            [JsonProperty("badge_tier", NullValueHandling = NullValueHandling.Ignore)]
            public int BadgeTier { get; set; }
            [JsonProperty("chat_message", NullValueHandling = NullValueHandling.Ignore)]
            public string ChatMessage { get; set; }
            [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
            public string Time { get; set; }
        }
        #endregion

        #region Channel Points - Only Data
        public struct PubSubChannelPointsMessage
        {
            [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
            public string Timestamp { get; set; }
            [JsonProperty("redemption", NullValueHandling = NullValueHandling.Ignore)]
            public Redemption _Redemption { get; set; }
            public struct Redemption
            {
                [JsonProperty("channel_id", NullValueHandling = NullValueHandling.Ignore)]
                public string ChannelId { get; set; }
                [JsonProperty("redeemed_at", NullValueHandling = NullValueHandling.Ignore)]
                public string RedeemedAt { get; set; }
                [JsonProperty("reward", NullValueHandling = NullValueHandling.Ignore)]
                public object Reward { get; set; }
                [JsonProperty("user_input", NullValueHandling = NullValueHandling.Ignore)]
                public string UserInput { get; set; }
                [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
                public string Status { get; set; }
            }
        }
        #endregion

        #region Channel Subscriptions - Only Message Data
        public struct PubSubChannelSubscriptionsMessage
        {
            [JsonProperty("user_name", NullValueHandling = NullValueHandling.Ignore)]
            public string UserName { get; set; }
            [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
            public string DisplayName { get; set; }
            [JsonProperty("channel_name", NullValueHandling = NullValueHandling.Ignore)]
            public string ChannelName { get; set; }
            [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
            public string UserId { get; set; }
            [JsonProperty("channel_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ChannelId { get; set; }
            [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
            public string Time { get; set; }
            [JsonProperty("sub_plan", NullValueHandling = NullValueHandling.Ignore)]
            public string SubPlan { get; set; }
            [JsonProperty("sub_plan_name", NullValueHandling = NullValueHandling.Ignore)]
            public string SubPlanName { get; set; }
            [JsonProperty("months", NullValueHandling = NullValueHandling.Ignore)]
            public int Months { get; set; }
            [JsonProperty("multi_month_duration", NullValueHandling = NullValueHandling.Ignore)]
            public int MultiMonthDuration { get; set; }
            [JsonProperty("cumulative_months", NullValueHandling = NullValueHandling.Ignore)]
            public int CumulativeMonths { get; set; }
            [JsonProperty("streak_months", NullValueHandling = NullValueHandling.Ignore)]
            public int StreakMonths { get; set; }
            [JsonProperty("context", NullValueHandling = NullValueHandling.Ignore)]
            public string Context { get; set; }
            [JsonProperty("is_gift", NullValueHandling = NullValueHandling.Ignore)]
            public bool IsGift { get; set; }
            [JsonProperty("sub_message", NullValueHandling = NullValueHandling.Ignore)]
            public SubMessage _SubMessage { get; set; }
            public struct SubMessage
            {
                [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
                public string Message { get; set; }
                [JsonProperty("emotes", NullValueHandling = NullValueHandling.Ignore)]
                public List<Emote> Emotes { get; set; }
                public struct Emote
                {
                    [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
                    public int Start { get; set; }
                    [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
                    public int End { get; set; }
                    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
                    public int Id { get; set; }
                }
            }
            [JsonProperty("recipient_id", NullValueHandling = NullValueHandling.Ignore)]
            public string RecipientId { get; set; }
            [JsonProperty("recipient_user_name", NullValueHandling = NullValueHandling.Ignore)]
            public string RecipientUserName { get; set; }
            [JsonProperty("recipient_display_name", NullValueHandling = NullValueHandling.Ignore)]
            public string RecipientDisplayName { get; set; }
        }
        #endregion

        #region User Moderation Notification - Only Message Data
        public struct PubSubUserModerationNotificationMessage
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }
            [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
            public string Status { get; set; }
        }
        #endregion

        #region AutoMod Queue - Only Message Data
        public struct PubSubAutoModQueueMessage
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }
            [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
            public Content _Content { get; set; }
            public struct Content
            {
                [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
                public string Text { get; set; }
                [JsonProperty("fragments", NullValueHandling = NullValueHandling.Ignore)]
                public List<object> _Fragments { get; set; }
            }
            [JsonProperty("sender", NullValueHandling = NullValueHandling.Ignore)]
            public Sender _Sender { get; set; }
            public struct Sender
            {
                [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
                public int UserId { get; set; }
                [JsonProperty("login", NullValueHandling = NullValueHandling.Ignore)]
                public string Login { get; set; }
                [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
                public string DisplayName { get; set; }
                [JsonProperty("chat_color", NullValueHandling = NullValueHandling.Ignore)]
                public string ChatColor { get; set; }
            }
            [JsonProperty("sent_at", NullValueHandling = NullValueHandling.Ignore)]
            public string SentAt { get; set; }
        }
        #endregion

        #region AutoMod Queue - Only Content Classification Data
        public struct PubSubAutoModQueueContentClassification
        {
            [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
            public string Category { get; set; }
            [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
            public int Level { get; set; }
        }
        #endregion

        #region AutoMod Queue - Only Status Data
        public struct PubSubAutoModQueueStatus
        {
            [JsonProperty("reason_code", NullValueHandling = NullValueHandling.Ignore)]
            public string ReasonCode { get; set; }
            [JsonProperty("resolver_id", NullValueHandling = NullValueHandling.Ignore)]
            public string ResolverId { get; set; }
            [JsonProperty("resolver_login", NullValueHandling = NullValueHandling.Ignore)]
            public string ResolverLogin { get; set; }
        }
        #endregion
    }
}