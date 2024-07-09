using ACBSChatbotConnector.Helpers;
using ACBSChatbotConnector.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var firstError = context.ModelState.Values
                               .SelectMany(v => v.Errors)
                               .Select(e => e.ErrorMessage)
                               .FirstOrDefault();

            if (firstError != null)
            {
                BaseResponse<object> _res;
                try
                {
                    _res = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<object>>(firstError);
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    _res = new BaseResponse<object>
                    {
                        Code = 502,
                        Message = "You must enter the parameters to be passed.",
                        Data = null
                    };
                }

                context.Result = new JsonResult(_res)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            else
            {
                context.Result = new JsonResult(new BaseResponse<object>
                {
                    Code = 501,
                    Message = "Invalid input.",
                    Data = null
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
    public void OnActionExecuted(ActionExecutedContext context) { }
}
