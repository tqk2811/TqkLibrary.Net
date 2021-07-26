using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace TqkLibrary.Net.ChoTot
{
  public class ChoTotApi
  {
    const string EndPoint = "https://gateway.chotot.com";
    static HttpClient httpClient { get { return NetExtensions.httpClient; } }

    readonly CancellationToken cancellationToken;
    readonly OauthResponse oauthResponse;
    public ChoTotApi(OauthResponse oauthResponse, CancellationToken cancellationToken = default)
    {
      this.oauthResponse = oauthResponse;
      this.cancellationToken = cancellationToken;
    }

    #region static
    public static async Task<OauthResponse> Login(AccountChoTot accountChoTot, CancellationToken cancellationToken = default)
    {
      if (accountChoTot == null) throw new ArgumentNullException(nameof(accountChoTot));
      if (string.IsNullOrEmpty(accountChoTot.Phone)) throw new ArgumentNullException(nameof(accountChoTot.Phone));
      if (string.IsNullOrEmpty(accountChoTot.Pass)) throw new ArgumentNullException(nameof(accountChoTot.Pass));

      HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{EndPoint}/v1/public/auth/login");
      httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(new { phone = accountChoTot.Phone, password = accountChoTot.Pass }), Encoding.UTF8, "application/json");
      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
      if (!httpResponseMessage.IsSuccessStatusCode) throw new HttpException((int)httpResponseMessage.StatusCode, await httpResponseMessage.Content.ReadAsStringAsync()); 
      return JsonConvert.DeserializeObject<OauthResponse>(await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync());
    }

    public static async Task<List<Region>> Regions(CancellationToken cancellationToken = default)
    {
      HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{EndPoint}/v2/public/chapy-pro/regions");
      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
      if (!httpResponseMessage.IsSuccessStatusCode) throw new HttpException((int)httpResponseMessage.StatusCode, await httpResponseMessage.Content.ReadAsStringAsync()); 
      return DynamicJsonConverter.ConvertRegion(await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync());
    }

    public static async Task<Wards> Wards(string areaId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(areaId)) throw new ArgumentNullException(nameof(areaId));

      HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{EndPoint}/v2/public/chapy-pro/wards?area={areaId}");
      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
      if (!httpResponseMessage.IsSuccessStatusCode) throw new HttpException((int)httpResponseMessage.StatusCode, await httpResponseMessage.Content.ReadAsStringAsync());
      return JsonConvert.DeserializeObject<Wards>(await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync());
    }
    #endregion

    public async Task RefreshToken()
    {
      if (oauthResponse == null) throw new ArgumentNullException(nameof(oauthResponse));
      if (string.IsNullOrEmpty(oauthResponse.refresh_token)) throw new ArgumentNullException(nameof(oauthResponse.refresh_token));
      HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{EndPoint}/v1/public/auth/token");
      httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(new { refresh_token = oauthResponse.refresh_token }), Encoding.UTF8, "application/json");
      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
      if (!httpResponseMessage.IsSuccessStatusCode) throw new HttpException((int)httpResponseMessage.StatusCode, await httpResponseMessage.Content.ReadAsStringAsync());
      OauthResponse newOauth = JsonConvert.DeserializeObject<OauthResponse>(await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync());
      oauthResponse.refresh_token = newOauth.refresh_token;
      oauthResponse.access_token = newOauth.access_token;
    }

    public async Task<FlashAdResponse> FlashAd(FlashAd flashAd)
    {
      if (flashAd == null) throw new ArgumentNullException(nameof(flashAd));
      HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{EndPoint}/v2/private/flashad/new");
      httpRequestMessage.Headers.Referrer = new Uri("https://www.chotot.com/");
      httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.128 Safari/537.36");
      httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", oauthResponse.access_token);
      string json = JsonConvert.SerializeObject(flashAd, NetExtensions.JsonSerializerSettings);
      Console.WriteLine(json);
      httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
      if (!httpResponseMessage.IsSuccessStatusCode) throw new HttpException((int)httpResponseMessage.StatusCode, await httpResponseMessage.Content.ReadAsStringAsync());
      return JsonConvert.DeserializeObject<FlashAdResponse>(await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync());
    }
    public Task<ImageResponse> UploadImage(Bitmap image)
      => UploadImage(image.BitmapToBuffer());

    public async Task<ImageResponse> UploadImage(byte[] image)
    {
      HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://cloudgw.chotot.com/v1/private/images/upload");
      httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", oauthResponse.access_token);
      using MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
      using ByteArrayContent byteArrayContent = new ByteArrayContent(image);
      multipartFormDataContent.Add(byteArrayContent, "image", "image.png");
      httpRequestMessage.Content = multipartFormDataContent;
      HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
      if (!httpResponseMessage.IsSuccessStatusCode) throw new HttpException((int)httpResponseMessage.StatusCode, await httpResponseMessage.Content.ReadAsStringAsync());
      return JsonConvert.DeserializeObject<ImageResponse>(await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync());
    }
  }
}