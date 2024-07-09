namespace ACBSChatbotConnector.Models.Request
{
    public class UserRoleCreateRequest
    {
        [CustomRequired(411)]
        public int UserId { get; set; }
        [CustomRequired(408)]
        public IEnumerable<int> RoleIds { get; set; }
    }
}
