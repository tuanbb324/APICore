namespace ACBSChatbotConnector.Models.Request
{
    public class Channel_UpdateChannelRequest
    {
        [CustomRequired(107)]
        public int Id { get; set; }
        [CustomRequired(108)]
        public string ChannelCode { get; set; }
        [CustomRequired(109)]
        public string ChannelName { get; set; }
        [CustomRequired(110)]
        public string LinkChannel { get; set; }
        [CustomRequired(111)]
        public string Avatar { get; set; }
    }
}
