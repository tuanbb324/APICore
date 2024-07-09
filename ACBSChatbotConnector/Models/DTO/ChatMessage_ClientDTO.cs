namespace ACBSChatbotConnector.Models.DTO
{
    public class ChatMessage_ClientDTO : CMS_User
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? TicketId { get; set; }
        public string TicketContent { get; set; }
        public string TicketStatus { get; set; }
        public string Content { get; set; }
        public string MsgType { get; set; }
        public string Command { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string Status { get; set; }
    }
}
