namespace ACBSChatbotConnector.Models.DTO
{
    public class CMS_User_Role_UpsertDto
    {
            public int UserId { get; set; }
            public int RoleId { get; set; }
            public int CreatedBy { get; set; }
            public int UpdatedBy { get; set; }
    }
}
