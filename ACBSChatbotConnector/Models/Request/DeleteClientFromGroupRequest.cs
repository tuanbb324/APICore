namespace ACBSChatbotConnector.Models.Request
{
    public class DeleteClientFromGroupRequest
    {
        public int? ClientId { get; set; }
        public int? GroupId { get; set; }
    }
}
