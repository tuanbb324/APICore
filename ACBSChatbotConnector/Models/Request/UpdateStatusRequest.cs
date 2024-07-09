namespace ACBSChatbotConnector.Models.Request
{
    public class UpdateStatusRequest
    {
        [CustomRequired(407)]
        public string Status { get; set; }
    }
}
