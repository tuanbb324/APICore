using ACBSChatbotConnector.Helpers.Validation;

namespace ACBSChatbotConnector.Models.Request
{
    public class CreateGroupRequest
    {
        [CustomRequired(901)]
        [CustomStringLength(50, 3, 4001)]
        public string GroupName { get; set; }
        [CustomRequired(901)]
        [CustomStringLength(50, 3, 4002)]
        public int ParentGroup { get; set; }
     
    }
}
