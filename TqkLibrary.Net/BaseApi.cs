using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace TqkLibrary.Net
{
  public abstract class BaseApi
  {
    protected readonly string ApiKey;

    //internal BaseApi() { }

    internal BaseApi(string ApiKey)
    {
      if (string.IsNullOrEmpty(ApiKey)) throw new ArgumentNullException(nameof(ApiKey));
      this.ApiKey = ApiKey;
    }

    protected async Task<T> RequestGet<T>(string url) => await RequestGet<T>(new Uri(url)).ConfigureAwait(false);

    protected async Task<T> RequestGet<T>(Uri uri)
    {
      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
      httpRequestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
      string content_res = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
      return JsonConvert.DeserializeObject<T>(content_res);
    }

    protected async Task<T> RequestPost<T>(string url, HttpContent httpContent) => await RequestPost<T>(new Uri(url), httpContent).ConfigureAwait(false);

    protected async Task<T> RequestPost<T>(Uri uri, HttpContent httpContent)
    {
      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
      httpRequestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      httpRequestMessage.Content = httpContent;
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
      string content_res = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
      return JsonConvert.DeserializeObject<T>(content_res);
    }

    protected async Task<T> RequestPost<T>(HttpRequestMessage httpRequestMessage)
    {
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
      string content_res = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
      return JsonConvert.DeserializeObject<T>(content_res);
    }
  }
}