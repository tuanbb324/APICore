namespace ACBSChatbotConnector.Models.Response
{
    public class BaseResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }
    public static class BaseRespondUtils
    {
        public static BaseResponse<T> SuccessRespond<T>(this object? obj)
        {
            BaseResponse<T> baseRespond = new BaseResponse<T>();
            baseRespond.Code = 200;
            baseRespond.Message = "success";
            baseRespond.Data = (T)obj;

            return baseRespond;
        }

        public static BaseResponse<T> ErrorRespond<T>(this object? obj, int respCode, string respMsg)
        {
            BaseResponse<T> baseRespond = new BaseResponse<T>();
            baseRespond.Code = respCode;
            baseRespond.Message = respMsg;
            baseRespond.Data = (T)obj;

            return baseRespond;
        }

        public static BaseResponse<T> InternalServerError<T>(this object? obj)
        {
            BaseResponse<T> baseRespond = new BaseResponse<T>();
            baseRespond.Code = 500;
            baseRespond.Message = "Internal server error.";
            baseRespond.Data = (T)obj;

            return baseRespond;
        }
    }
}
