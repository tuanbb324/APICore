namespace ACBSChatbotConnector.Models.DTO
{
    public class Chat_GetArchivedMessagesDTO
    {
        public int Id { get; set; }
        public string ClientAvatar { get; set; }
        public string ClientName { get; set; }
        public DateTime ReceivedTime {  get; set; }
        public string Content { get; set; }
        public string ReceiverName { get; set; }
        public string ForwardedBy { get; set; }
        public string Status {  get; set; }
        public DateTime LastActivationTime { get; set; }

    }
}
