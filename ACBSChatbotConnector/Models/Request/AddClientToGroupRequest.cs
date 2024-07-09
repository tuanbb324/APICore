namespace ACBSChatbotConnector.Models.Request
{
    public class AddClientToGroupRequest
    {
        [CustomRequired(911)]
        public List<int> ClientIds { get; set; }
        [CustomRequired(912)]
        public int GroupId { get; set; }
    }
}
