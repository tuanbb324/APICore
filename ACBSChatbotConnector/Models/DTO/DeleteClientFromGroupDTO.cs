namespace ACBSChatbotConnector.Models.DTO
{
    public class DeleteClientFromGroupDTO
    {
        public int? ClientId { get; set; }
        public int? GroupId { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
