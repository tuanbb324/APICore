namespace ACBSChatbotConnector.Models.Request
{
    public class UpdateClientById
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string TagName { get; set; }
        public string Gender { get; set; }
    }
}
