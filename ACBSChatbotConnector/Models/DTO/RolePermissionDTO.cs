namespace ACBSChatbotConnector.Models.Request
{
    public class RolePermissionDTO
    {
        public int RoleId { get; set; }
        public int PermissionIds { get; set; }
        public int CreatedBy { get; set; }
    }
}
