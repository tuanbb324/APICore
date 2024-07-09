using ACBSChatbotConnector.Helpers.Validation;
using System.Diagnostics.CodeAnalysis;

namespace ACBSChatbotConnector.Models.Requests
{
    public class CMS_CreateUserRequest
    {
        [CustomRequired(100)]
        [CustomStringLength(30, 3, 1001)]
        public string Username { get; set; }

        //[CustomRequired(101)]
        //[CustomStringLength(30, 3, 1011)]
        //public string Password { get; set; }

        [CustomRequired(102)]
        [CustomStringLength(50, 1, 1021)]
        public string FullName { get; set; }

        [CustomRequired(103)]
        [CustomStringLength(50, 5, 1031)]
        public string Email { get; set; }

        [CustomRequired(408)]
        public int RoleId { get; set; }
    }
}
