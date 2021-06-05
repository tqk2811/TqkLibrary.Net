using Newtonsoft.Json;

namespace TqkLibrary.Net.Captcha.AntiCaptchaCom
{
  public sealed partial class AntiCaptchaApi
  {
    private class TaskResultJson
    {
      [JsonProperty("clientKey")]
      public string ClientKey { get; set; }

      [JsonProperty("taskId")]
      public int TaskId { get; set; }
    }
  }
}