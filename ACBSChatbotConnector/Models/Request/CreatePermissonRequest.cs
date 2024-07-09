using ACBSChatbotConnector.Helpers.Validation;

namespace ACBSChatbotConnector.Models.Requests
{
    public class CreatePermissonRequest
    {
        [CustomRequired(405)]
        [CustomStringLength(100, 3, 2006)]
        public string PermissionName { get; set; }
        [CustomRequired(406)]
        [CustomStringLength(100, 3, 2007)]
        public string Detail { get; set; }
    }
}
