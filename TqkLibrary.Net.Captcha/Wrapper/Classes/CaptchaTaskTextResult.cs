using System.Diagnostics.CodeAnalysis;

namespace TqkLibrary.Net.Captcha.Wrapper.Classes
{
    public class CaptchaTaskTextResult
    {
        public required bool IsSuccess { get; set; }
        public string? Value { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
