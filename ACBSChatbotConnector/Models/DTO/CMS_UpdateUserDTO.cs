namespace ACBSChatbotConnector.Models.DTO
{
    public class CMS_UpdateUserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }

    }
}
