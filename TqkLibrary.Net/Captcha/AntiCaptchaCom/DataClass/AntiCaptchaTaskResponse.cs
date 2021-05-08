using Newtonsoft.Json;

namespace TqkLibrary.Net.Captcha.AntiCaptchaCom
{
  public sealed class AntiCaptchaTaskResponse
  {
    [JsonProperty("errorId")]
    public int? ErrorId { get; set; }

    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; }

    [JsonProperty("errorDescription")]
    public string ErrorDescription { get; set; }

    [JsonProperty("taskId")]
    public int? TaskId { get; set; }
  }
}