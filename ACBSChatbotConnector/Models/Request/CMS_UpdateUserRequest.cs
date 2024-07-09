using ACBSChatbotConnector.Helpers.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ACBSChatbotConnector.Models.Requests
{
    public class CMS_UpdateUserRequest
    {
        [CustomRequired(107)]
        public int Id { get; set; }

        [CustomRequired(102)]
        [CustomStringLength(50, 1, 1021)]
        public string FullName { get; set; }

        [CustomRequired(408)]
        public int RoleId { get; set; }

    }
}
