using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace TqkLibrary.Net
{
  public class ApiException<T> : Exception
  {
    public ApiException()
    {

    }

    public HttpStatusCode StatusCode { get; internal set; }
    public T Body { get; internal set; }
  }
  public abstract class BaseApi
  {
    static readonly Type typeString = typeof(string);
    static readonly Type typeBuffer = typeof(byte[]);

    protected readonly string ApiKey;
    protected readonly CancellationToken cancellationToken;
    internal BaseApi(CancellationToken cancellationToken = default)
    {
      this.cancellationToken = cancellationToken;
    }

    internal BaseApi(string ApiKey, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(ApiKey)) throw new ArgumentNullException(nameof(ApiKey));
      this.ApiKey = ApiKey;
      this.cancellationToken = cancellationToken;
    }

    protected Task<TResult> RequestGet<TResult>(
      string url,
      Dictionary<string, string> headers = null,
      HttpContent httpContent = null)
      where TResult : class
      => Request<TResult, string>(HttpMethod.Get, new Uri(url), headers, httpContent);
    protected Task<TResult> RequestGet<TResult>(
     Uri uri,
     Dictionary<string, string> headers = null,
     HttpContent httpContent = null)
     where TResult : class
     => Request<TResult, string>(HttpMethod.Get, uri, headers, httpContent);
    protected Task<TResult> RequestGet<TResult, TException>(
      string url,
      Dictionary<string, string> headers = null,
      HttpContent httpContent = null)
      where TResult : class
      where TException : class
      => Request<TResult, TException>(HttpMethod.Get, new Uri(url), headers, httpContent);
    protected Task<TResult> RequestGet<TResult, TException>(
      Uri uri,
      Dictionary<string, string> headers = null,
      HttpContent httpContent = null)
      where TResult : class
      where TException : class
      => Request<TResult, TException>(HttpMethod.Get, uri, headers, httpContent);


    protected Task<TResult> RequestPost<TResult>(
      string url,
      Dictionary<string, string> headers = null,
      HttpContent httpContent = null)
      where TResult : class
      => Request<TResult, string>(HttpMethod.Post, new Uri(url), headers, httpContent);
    protected Task<TResult> RequestPost<TResult>(
     Uri uri,
     Dictionary<string, string> headers = null,
     HttpContent httpContent = null)
     where TResult : class
     => Request<TResult, string>(HttpMethod.Post, uri, headers, httpContent);
    protected Task<TResult> RequestPost<TResult, TException>(
      string url,
      Dictionary<string, string> headers = null,
      HttpContent httpContent = null)
      where TResult : class
      where TException : class
      => Request<TResult, TException>(HttpMethod.Post, new Uri(url), headers, httpContent);
    protected Task<TResult> RequestPost<TResult, TException>(
     Uri uri,
     Dictionary<string, string> headers = null,
     HttpContent httpContent = null)
     where TResult : class
     where TException : class
     => Request<TResult, TException>(HttpMethod.Post, uri, headers, httpContent);


    protected async Task<TResult> Request<TResult, TException>(
      HttpMethod method,
      Uri uri,
      Dictionary<string, string> headers = null,
      HttpContent httpContent = null) 
      where TResult : class 
      where TException: class
    {
      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, uri);
      foreach (var pair in headers) httpRequestMessage.Headers.Add(pair.Key, pair.Value);
      if(headers != null) httpRequestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      if (httpContent != null) httpRequestMessage.Content = httpContent;
      using HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
      
      if (httpResponseMessage.IsSuccessStatusCode)
      {
        if (typeof(TResult).Equals(typeBuffer))
        {
          return (await httpResponseMessage.Content.ReadAsByteArrayAsync()) as TResult;
        }
        else
        {
          string content_res = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
          if (typeof(TResult).Equals(typeString))
          {
            return content_res as TResult;
          }
          //else if(typeof(TResult).IsSubclassOf(typeStream))
          //{
          //  return (await httpResponseMessage.Content.ReadAsStreamAsync()) as TResult;
          //}
          else
            return JsonConvert.DeserializeObject<TResult>(content_res);
        }
      }
      else
      {
        string content_res = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (typeof(TException).Equals(typeString))
        {
          throw new ApiException<string>()
          {
            Body = content_res,
            StatusCode = httpResponseMessage.StatusCode
          };
        }
        else throw new ApiException<TException>()
        {
          Body = JsonConvert.DeserializeObject<TException>(content_res),
          StatusCode = httpResponseMessage.StatusCode
        };
      }
    }

  }
}