using ACBSChatbotConnector.Helpers;
using ACBSChatbotConnector.Models;


namespace ACBSChatbotConnector.Services.Implement
{
    public class FileUploadService : IFileUploadService
    {
        public FileUploadService()
        {
        }
        public FileUpload UpFile(IFormFile postedFile, ref string _fileNameNotAccept)
        {
            FileUpload fileUpload = new FileUpload();

            string prefix = DateTime.UtcNow.ToString("yyMMddHHmmss");
            string fileEx = Path.GetExtension(postedFile.FileName).ToLower();
            fileUpload.FileType = "IMAGE";

            if (!Config.Config.IMAGE_EXTENSION_ACCEPT.Contains(fileEx))
            {
                fileUpload.FileType = "OTHER";
                _fileNameNotAccept = postedFile.FileName;

                return null;
            }

            string root = Config.Config.RootFolder;
            DateTime creationTime = DateTime.UtcNow;
            string datetime = creationTime.ToString("yyyyMMdd");
            string url = Config.Config.UPLOAD_MEDIA_FOLDER + @"\" + datetime + @"\" + prefix + "_" + fileEx;
            if (!Directory.Exists(@"\" + datetime))
            {
                Directory.CreateDirectory(root + Config.Config.UPLOAD_MEDIA_FOLDER + @"\" + datetime);
            }
            string filePath = Config.Config.RootFolder + url;

            fileUpload.FileName = postedFile.FileName;
            fileUpload.FilePath = url;
            fileUpload.FileType = "IMAGE";
            fileUpload.FileExtension = fileEx;
            fileUpload.FileSize = postedFile.Length.ToFloat() / 1024;
            fileUpload.CreatedDate = DateTime.UtcNow;

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                postedFile.CopyTo(fileStream);
            }

            return fileUpload;
        }
        
    }
}
