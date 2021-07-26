using Newtonsoft.Json;

namespace TqkLibrary.Net.Captcha
{
  public sealed class AntiCaptchaTaskSolutionResultResponse
  {
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    public string gRecaptchaResponse { get; set; }
  }
}