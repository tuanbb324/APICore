namespace ACBSChatbotConnector.Models.DTO
{
    public class RolePermission_CreateDTO
    {
        public int RoleId { get; set; }
        public IEnumerable<int> PermissionId { get; set; }
        public int CreatedBy { get; set; }
    }
}
