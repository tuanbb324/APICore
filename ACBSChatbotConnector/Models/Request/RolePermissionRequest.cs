using ACBSChatbotConnector.Helpers.Validation;

namespace ACBSChatbotConnector.Models.Request
{
    public class RolePermissionRequest
    {
        [CustomRequired(408)]
        public int RoleId { get; set; }
        [CustomRequired(409)]
        public IEnumerable<int> PermissionIds { get; set; }
    }
}
