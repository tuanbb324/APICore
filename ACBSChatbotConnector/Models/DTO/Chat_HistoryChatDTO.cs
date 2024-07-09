using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.DTO;


namespace ChatService.Models.Response
{
    public class Chat_HistoryChatDTO
    {
        public int? UserId { get; set; }
        public Chat_HistoryChatUserInforDTO Sender { get; set; }
        public int? TicketId { get; set; }
        public string Content { get; set; }
        public string MsgType { get; set; }
        public string Command { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string? Status { get; set; }
    }
}
