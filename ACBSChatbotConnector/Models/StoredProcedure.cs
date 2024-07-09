using System.ComponentModel.DataAnnotations;

namespace ACBSChatbotConnector.Model
{
    public class StoredProcedure
    {
        public int? UserId { get; set; }
        public int? TicketId { get; set; }
        public string Content { get; set; }
        public string MsgType { get; set; }
        public string Command { get; set; }
    }
}
