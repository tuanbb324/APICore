namespace ACBSChatbotConnector.Models
{
    public class CMS_User_RefreshTokens
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
