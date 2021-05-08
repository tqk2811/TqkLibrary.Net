using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TqkLibrary.Net.Captcha.TwoCaptchaCom
{
  public sealed class TwoCaptchaApi : BaseApi
  {
    private const string EndPoint = "https://2captcha.com";

    public TwoCaptchaApi(string ApiKey) : base(ApiKey)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    public Task<TwoCaptchaResponse> GetResponseJson(string id)
      => RequestGet<TwoCaptchaResponse>(EndPoint + string.Format("/res.php?key={0}&id={1}&action=get&json=1", ApiKey, id));

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="delay"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="JsonException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public async Task<TwoCaptchaResponse> WaitResponseJsonCompleted(string id, CancellationToken cancellationToken, int delay = 5000)
    {
      while (true)
      {
        Task.Delay(delay, cancellationToken).Wait();
        TwoCaptchaResponse twoCaptchaResponse = await GetResponseJson(id).ConfigureAwait(false);
        switch (twoCaptchaResponse.CheckState())
        {
          case TwoCaptchaState.NotReady:
            continue;
          case TwoCaptchaState.Error:
          case TwoCaptchaState.Success:
            return twoCaptchaResponse;
        }
      }
    }

    //https://2captcha.com/2captcha-api#solving_recaptchav2_old
    public Task<TwoCaptchaResponse> ReCaptchaV2_old(Bitmap bitmap, Bitmap imginstructions, int? recaptcharows = null, int? recaptchacols = null)
    {
      if (null == bitmap) throw new ArgumentNullException(nameof(bitmap));
      if (null == imginstructions) throw new ArgumentNullException(nameof(imginstructions));

      byte[] buffer_bitmap = null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        bitmap.Save(memoryStream, ImageFormat.Jpeg);//hoac png
        memoryStream.Position = 0;
        buffer_bitmap = new byte[memoryStream.Length];
        memoryStream.Read(buffer_bitmap, 0, (int)memoryStream.Length);
      }

      byte[] buffer_instructions = null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        imginstructions.Save(memoryStream, ImageFormat.Jpeg);//hoac png
        memoryStream.Position = 0;
        buffer_instructions = new byte[memoryStream.Length];
        memoryStream.Read(buffer_instructions, 0, (int)memoryStream.Length);
      }

      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["key"] = ApiKey;
      parameters["recaptcha"] = "1";
      parameters["method"] = "post";
      parameters["json"] = "1";
      //if(!string.IsNullOrEmpty(textinstructions)) parameters["textinstructions"] = textinstructions;
      if (recaptcharows != null) parameters["recaptcharows"] = recaptcharows.Value.ToString();
      if (recaptchacols != null) parameters["recaptchacols"] = recaptchacols.Value.ToString();
      Uri uri = new Uri(EndPoint + "/in.php?" + parameters.ToString());

      MultipartFormDataContent requestContent = new MultipartFormDataContent();
      ByteArrayContent imageContent_bitmap = new ByteArrayContent(buffer_bitmap);
      imageContent_bitmap.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
      requestContent.Add(imageContent_bitmap, "file", "file.jpg");
      ByteArrayContent imageContent_instructions = new ByteArrayContent(buffer_instructions);
      imageContent_instructions.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
      requestContent.Add(imageContent_instructions, "imginstructions", "imginstructions.jpg");

      return RequestPost<TwoCaptchaResponse>(uri, requestContent);
    }

    public Task<TwoCaptchaResponse> Nomal(Bitmap bitmap)
    {
      byte[] buffer_bitmap = null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        bitmap.Save(memoryStream, ImageFormat.Jpeg);//hoac png
        memoryStream.Position = 0;
        buffer_bitmap = new byte[memoryStream.Length];
        memoryStream.Read(buffer_bitmap, 0, (int)memoryStream.Length);
      }
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["key"] = ApiKey;
      parameters["method"] = "post";
      parameters["json"] = "1";
      Uri uri = new Uri(EndPoint + "/in.php?" + parameters.ToString());

      MultipartFormDataContent requestContent = new MultipartFormDataContent();
      ByteArrayContent imageContent_bitmap = new ByteArrayContent(buffer_bitmap);
      imageContent_bitmap.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
      requestContent.Add(imageContent_bitmap, "file", "file.jpg");

      return RequestPost<TwoCaptchaResponse>(uri, requestContent);
    }

    //https://2captcha.com/2captcha-api#recaptchav2new_proxy
    public Task<TwoCaptchaResponse> RecaptchaV2(string googleKey, string pageUrl, string cookies = null, string proxy = null, string proxytype = null)
    {
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["key"] = ApiKey;
      parameters["googlekey"] = googleKey;
      parameters["method"] = "userrecaptcha";
      parameters["json"] = "1";
      parameters["pageurl"] = pageUrl;
      if (!string.IsNullOrEmpty(cookies)) parameters["cookies"] = cookies;
      if (!string.IsNullOrEmpty(proxy)) parameters["proxy"] = proxy;
      if (!string.IsNullOrEmpty(proxytype)) parameters["proxytype"] = proxytype;
      Uri uri = new Uri(EndPoint + "/in.php?" + parameters.ToString());

      return RequestGet<TwoCaptchaResponse>(uri);
    }
  }
}