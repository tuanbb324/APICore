using System.ComponentModel.DataAnnotations;

namespace ACBSChatbotConnector.Models.Request
{
    public class SendMessageRequest
    {
        public int? UserId { get; set; }
        public string ClientId { get; set; }
        public string Content { get; set; }
        public string MsgType { get; set; }
        public string Command { get; set; }
    }
}
