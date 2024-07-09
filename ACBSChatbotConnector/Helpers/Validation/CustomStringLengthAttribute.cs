using System.ComponentModel.DataAnnotations;

namespace ACBSChatbotConnector.Helpers.Validation
{
    public class CustomStringLengthAttribute : ValidationAttribute
    {
        private readonly int _maxLength;
        private readonly int _minLength;
        private readonly int _errorCode;

        public CustomStringLengthAttribute(int maxLength, int minLength, int errorCode)
        {
            _maxLength = maxLength;
            _minLength = minLength;
            _errorCode = errorCode;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var stringValue = value.ToString();
            var length = stringValue.Length;

            if (length < _minLength || length > _maxLength)
            {
                var result = new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                string _key = validationContext.MemberName;
                string _errMsg;
                if (string.IsNullOrEmpty(result.ErrorMessage))
                    _errMsg = $"The value of '{_key}' must be between {_minLength} and {_maxLength} characters long.";
                else
                    _errMsg = result.ErrorMessage;

                result.ErrorMessage = FormatErrorMessage(_errorCode, _errMsg);
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
