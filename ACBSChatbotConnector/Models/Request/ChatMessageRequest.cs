namespace ACBSChatbotConnector.Models
{
    public class ChatMessageRequest
    {
        public string ClientId { get; set; }
        public string Content { get; set; }
        public string MsgType { get; set; }
        public string Command { get; set; }
    }
}
