namespace ACBSChatbotConnector.Models.DTO
{
    public class AddGroupDTO
    {
        public string GroupName { get; set; }
        public int? ParentGroup { get; set; }
        public int CreatedBy { get; set; }
    }
}
