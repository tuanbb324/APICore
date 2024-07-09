namespace ACBSChatbotConnector.Models
{
    public class Permission
    {
        public int Id { get; set; } 
        public string PermissionName { get; set; }    
        public string Detail { get; set; }    
        public DateTime CreatedTime { get; set; }    
        public DateTime? UpdatedTime { get; set; }    
        public int CreatedBy { get; set; }    
        public int? UpdatedBy { get; set; }
        public string Status { get; set; }
    }
}
