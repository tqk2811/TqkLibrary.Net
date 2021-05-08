namespace TqkLibrary.Net.Captcha.TwoCaptchaCom
{
  public sealed class TwoCaptchaResponse
  {
    public int status { get; set; }
    public string request { get; set; }

    public override string ToString()
    {
      return $"request: {status}, request: {request}";
    }

    public TwoCaptchaState CheckState()
    {
      if (status == 1) return TwoCaptchaState.Success;
      if (request.Contains("CAPCHA_NOT_READY")) return TwoCaptchaState.NotReady;
      else return TwoCaptchaState.Error;
    }
  }
}