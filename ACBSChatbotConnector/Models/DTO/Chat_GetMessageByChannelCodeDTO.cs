namespace ACBSChatbotConnector.Models.DTO
{
    public class Chat_GetMessageByChannelCodeDTO
    {
        public int? UserId { get; set; }
        public Chat_HistoryChatUserInforDTO Sender { get; set; }
        public int? TicketId { get; set; }
        public Chat_GetMessageByChannelCodeTicketInforDTO Ticket { get; set; }
        public string Content { get; set; }
        public string MsgType { get; set; }
        public string Command { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string? Status { get; set; }
    }
}
