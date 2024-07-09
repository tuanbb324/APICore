using static ACBSChatbotConnector.Helpers.Extensions;
using System.ComponentModel.DataAnnotations;

namespace ACBSChatbotConnector.Helpers.Validation
{
    public class StatusValidationAttribute : ValidationAttribute
    {
        private int _errCode;
        public StatusValidationAttribute(int errCode)
        {
            _errCode = errCode;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _value = value.ToString();
            if (_value != Status.Active.ToString()
                && _value != Status.InActive.ToString()
                && _value != Status.Pending.ToString()
                && _value != Status.Inprogress.ToString()
                && _value != Status.Deleted.ToString()
                )
            {
                string _key = validationContext.MemberName;
                var result = new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                result.ErrorMessage = FormatErrorMessage(_errCode, $"The field {_key} must be Active, InActive, Pending, Inprogress or Deleted.");
                return result;
            }

            return ValidationResult.Success;
        }

        private string FormatErrorMessage(int code, string message)
        {
            return $"{{\"code\": {code}, \"message\": \"{message}\", \"data\": null}}";
        }
    }
}
