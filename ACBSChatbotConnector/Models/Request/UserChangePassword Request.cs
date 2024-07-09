using ACBSChatbotConnector.Helpers.Validation;

namespace ACBSChatbotConnector.Models.Request
{
    public class UserChangePasswordRequest
    {
        [CustomRequired(131)]
        [CustomStringLength(30, 3, 1011)]
        public string NewPassword { get; set; }
    }
}
