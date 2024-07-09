namespace ACBSChatbotConnector.Models.DTO
{
    public class GetClientByTagDTO
    {
        public int? PartnerId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Status { get; set; }
        public DateTime? LastActive { get; set; }
        public int? ChannelId { get; set; }
        public string GroupName { get; set; }
    }
}
