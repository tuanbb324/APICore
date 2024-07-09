namespace ACBSChatbotConnector.Models.DTO
{
    public class UpdateGroupDTO
    {
        public string GroupName { get; set; }
        public int? ParentGroup { get; set; }
        public int UpdatedBy { get; set; }
    }
}
