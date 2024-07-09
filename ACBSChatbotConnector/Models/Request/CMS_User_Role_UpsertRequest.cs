namespace ACBSChatbotConnector.Models.Request
{
    public class CMS_User_Role_UpsertRequest
    {
        [CustomRequired(408)]
        public int RoleId { get; set; }
    }
}
