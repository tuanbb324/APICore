namespace ACBSChatbotConnector.Models.DTO
{
    public class AddClientToGroupATO
    {
        public string ClientId { get; set; }
        public int GroupId { get; set; }
        public int CreatedBy { get; set; }
    }
}