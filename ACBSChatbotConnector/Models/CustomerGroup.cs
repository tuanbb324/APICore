namespace ACBSChatbotConnector.Models
{
    public class CustomerGroup
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int? GroupId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string Status { get; set; }
    }
}
