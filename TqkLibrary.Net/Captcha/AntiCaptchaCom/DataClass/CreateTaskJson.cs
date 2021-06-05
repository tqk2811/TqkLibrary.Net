using Newtonsoft.Json;

namespace TqkLibrary.Net.Captcha.AntiCaptchaCom
{
  public sealed partial class AntiCaptchaApi
  {
    private class CreateTaskJson
    {
      [JsonProperty("clientKey")]
      public string ClientKey { get; set; }

      [JsonProperty("task")]
      public AntiCaptchaTask Task { get; set; }

      [JsonProperty("softId")]
      public int SoftId { get; set; } = 0;

      [JsonProperty("languagePool")]
      public string LanguagePool { get; set; } = "en";
    }
  }
}