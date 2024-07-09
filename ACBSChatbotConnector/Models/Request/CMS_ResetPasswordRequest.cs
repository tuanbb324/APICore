using ACBSChatbotConnector.Helpers.Validation;
using System.Diagnostics.CodeAnalysis;

namespace ACBSChatbotConnector.Models.Requests
{
    public class CMS_ResetPasswordRequest
    {
        [CustomRequired(100)]
        [CustomStringLength(30, 3, 1001)]
        public string Username { get; set; }

        [CustomRequired(101)]
        [CustomStringLength(30, 3, 1011)]
        public string Password { get; set; }
    }
}
