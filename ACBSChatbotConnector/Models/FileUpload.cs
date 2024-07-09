namespace ACBSChatbotConnector.Models
{
    public class FileUpload
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
        public float? FileSize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string FilePath { get; set; }
        public float? TimeDuration { get; set; }
        public string VideoThumbnail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string FileKey { get; set; }
    }
}
