using ACBSChatbotConnector.Models;

namespace ACBSChatbotConnector.Services
{
    public interface IFileUploadService
    {
        FileUpload UpFile(IFormFile postedFile, ref string _fileNameNotAccept);
    }
}
