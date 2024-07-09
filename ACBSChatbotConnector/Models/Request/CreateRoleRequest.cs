using ACBSChatbotConnector.Helpers.Validation;

namespace ACBSChatbotConnector.Models.Requests
{
    public class CreateRoleRequest
    {
        [CustomRequired(403)]
        [CustomStringLength(100, 3, 2004)]
        public string RoleName { get; set; }
        [CustomRequired(404)]
        [CustomStringLength(500, 3, 2005)]
        public string Detail { get; set; }
    }
}
