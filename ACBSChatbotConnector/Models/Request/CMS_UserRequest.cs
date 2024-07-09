namespace ACBSChatbotConnector.Models.Request
{
    public class CMS_UserRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public int RoleId { get; set; }
    }
}