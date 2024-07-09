namespace ACBSChatbotConnector.Models
{
    public class Notification 
    {
        public long OwnerId { get; set; }
        public string Message { get; set; }
        public string NotiType { get; set; }
        public char IsRead { get; set; }
        public long SourceId { get; set; }
    }
}
