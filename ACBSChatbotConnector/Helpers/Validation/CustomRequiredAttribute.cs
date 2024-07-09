using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class CustomRequiredAttribute : RequiredAttribute
{
    public int ErrorCode { get; set; }

    public CustomRequiredAttribute(int errorCode)
    {
        ErrorCode = errorCode;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        try
        {

            var result = base.IsValid(value, validationContext);
            if (result != ValidationResult.Success)
            {
                result = new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                string _key = validationContext.MemberName;
                string _errMsg;
                if (string.IsNullOrEmpty(result.ErrorMessage))
                    _errMsg = $"The field '{_key}' is required.";
                else
                    _errMsg = result.ErrorMessage;

                result.ErrorMessage = FormatErrorMessage(ErrorCode, _errMsg);
            }
            return result;
        }
        catch(Exception ex)
        {
            throw;
        }
    }

    private string FormatErrorMessage(int code, string message)
    {
        return $"{{\"code\": {code}, \"message\": \"{message}\", \"data\": null}}";
    }
}
