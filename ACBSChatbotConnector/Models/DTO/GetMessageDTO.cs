namespace ACBSChatbotConnector.Models.DTO
{
    public class GetMessageDTO
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? TicketId { get; set; }
        public string Content { get; set; }
        public string MsgType { get; set; }
        public string Command { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
