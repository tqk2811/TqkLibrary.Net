namespace TqkLibrary.Net.Captcha.AntiCaptchaCom
{
  /// <summary>
  /// https://anti-captcha.com/apidoc/image
  /// </summary>
  public enum AntiCaptchaType
  {
    FunCaptchaTask,
    FunCaptchaTaskProxyless,
    ImageToTextTask,

    /// <summary>
    /// Recaptcha no proxy
    /// </summary>
    NoCaptchaTaskProxyless,

    /// <summary>
    /// Recaptcha with proxy
    /// </summary>
    NoCaptchaTask,

    /// <summary>
    /// recaptcha V3 No proxy
    /// </summary>
    RecaptchaV3TaskProxyless,

    GeeTestTaskProxyless,
    GeeTestTask,
    HCaptchaTask,
    HCaptchaTaskProxyless
  }
}