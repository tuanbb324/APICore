using ACBSChatbotConnector.Helpers.Validation;

namespace ACBSChatbotConnector.Models.Request
{
    public class UpdateGroupRequest
    {
        [CustomRequired(901)]
        [CustomStringLength(50, 3, 4001)]
        public string GroupName { get; set; }
        public int? ParentGroup { get; set; }
    }
}