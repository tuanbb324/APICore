namespace ACBSChatbotConnector.Models.DTO
{
    public class GetAllGroupDTO
    {
        public int? Id { get; set; }
        public string? GroupName { get; set; }
        public string? ParentGroup { get; set; }
        public string? Detail { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
