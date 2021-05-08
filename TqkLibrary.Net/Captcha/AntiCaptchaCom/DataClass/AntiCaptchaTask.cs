using Newtonsoft.Json;

namespace TqkLibrary.Net.Captcha.AntiCaptchaCom
{
  /// <summary>
  /// https://anti-captcha.com/apidoc/image
  /// </summary>
  public sealed class AntiCaptchaTask
  {
    [JsonProperty("type")]
    public AntiCaptchaType Type { get; set; }

    [JsonProperty("websiteURL")]
    public string WebsiteUrl { get; set; }

    [JsonProperty("websiteKey")]
    public string WebsiteKey { get; set; }

    [JsonProperty("proxyType")]
    public string ProxyType { get; set; }

    [JsonProperty("proxyAddress")]
    public string ProxyAddress { get; set; }

    [JsonProperty("proxyPort")]
    public int? ProxyPort { get; set; }

    [JsonProperty("proxyLogin")]
    public string ProxyLogin { get; set; }

    [JsonProperty("proxyPassword")]
    public string ProxyPassword { get; set; }

    [JsonProperty("userAgent")]
    public string UserAgent { get; set; }

    [JsonProperty("cookies")]
    public string Cookies { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    [JsonProperty("funcaptchaApiJSSubdomain")]
    public string FunCaptchaApiJSSubDomain { get; set; }

    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("websitePublicKey")]
    public string WebsitePublicKey { get; set; }

    [JsonProperty("gt")]
    public string GT { get; set; }

    [JsonProperty("challenge")]
    public string Challenge { get; set; }

    [JsonProperty("geetestApiServerSubdomain")]
    public string GeeTestApiServerSubdomain { get; set; }
  }
}