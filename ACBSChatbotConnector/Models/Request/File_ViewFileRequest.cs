using System.Diagnostics.CodeAnalysis;

namespace ACBSChatbotConnector.Models.Requests
{
    public class File_ViewFileRequest
    {
        [NotNull]
        public string FileName { get; set; }

    }
}
