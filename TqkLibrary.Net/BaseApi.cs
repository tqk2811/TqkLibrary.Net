using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace TqkLibrary.Net
{
  public abstract class BaseApi
  {
    static readonly Type typeString = typeof(string);
    protected readonly string ApiKey;

    internal BaseApi() { }

    internal BaseApi(string ApiKey)
    {
      if (string.IsNullOrEmpty(ApiKey)) throw new ArgumentNullException(nameof(ApiKey));
      this.ApiKey = ApiKey;
    }

    protected async Task<T> RequestGet<T>(string url, CancellationToken cancellationToken = default) => await RequestGet<T>(new Uri(url), cancellationToken).ConfigureAwait(false);

    protected async Task<T> RequestGet<T>(Uri uri, CancellationToken cancellationToken = default)
    {
      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
      httpRequestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
      string content_res = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
      return JsonConvert.DeserializeObject<T>(content_res);
    }
    protected Task<HttpContent> RequestGetContent(string url, CancellationToken cancellationToken = default)
      => RequestGetContent(new Uri(url), cancellationToken);

    protected async Task<HttpContent> RequestGetContent(Uri uri, CancellationToken cancellationToken = default)
    {
      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
      return httpResponseMessage.EnsureSuccessStatusCode().Content;
    }


    protected async Task<T> RequestPost<T>(string url, HttpContent httpContent, CancellationToken cancellationToken = default) 
      => await RequestPost<T>(new Uri(url), httpContent,cancellationToken).ConfigureAwait(false);

    protected async Task<T> RequestPost<T>(Uri uri, HttpContent httpContent, CancellationToken cancellationToken = default)
    {
      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
      httpRequestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      httpRequestMessage.Content = httpContent;
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead,cancellationToken).ConfigureAwait(false);
      string content_res = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
      return JsonConvert.DeserializeObject<T>(content_res);
    }

    protected async Task<T> RequestPost<T>(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken = default)
    {
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
      string content_res = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
      return JsonConvert.DeserializeObject<T>(content_res);
    }
  }
}