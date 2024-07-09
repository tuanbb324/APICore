namespace ACBSChatbotConnector.Models.Response
{
    public class PagingResponse<T>
    {
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public T Data { get; set; }
    }
}
