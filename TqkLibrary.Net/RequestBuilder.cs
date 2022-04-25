using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestBuilder
    {
        internal RequestBuilder(BaseApi baseApi)
        {
            this.HttpClient = baseApi.httpClient;
        }
        internal RequestBuilder(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }
        HttpClient HttpClient { get; set; }
        CancellationToken cancellationToken = CancellationToken.None;
        HttpContent httpContent = null;
        bool httpContentDispose = true;

        Dictionary<string, string> headers = new Dictionary<string, string>();
        HttpMethod method = null;
        Uri uri = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public RequestBuilder WithUrl(UriBuilder uri, HttpMethod method)
        {
            return WithUrl((Uri)uri, method);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlGet(UriBuilder uri)
        {
            return WithUrl((Uri)uri, HttpMethod.Get);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlGet(string uri)
        {
            return WithUrl(new Uri(uri), HttpMethod.Get);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlGet(Uri uri)
        {
            return WithUrl(uri, HttpMethod.Get);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public RequestBuilder WithUrl(string uri, HttpMethod method)
        {
            return WithUrl(new Uri(uri), method);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public RequestBuilder WithUrl(Uri uri, HttpMethod method)
        {
            this.uri = uri ?? throw new ArgumentNullException(nameof(uri));
            this.method = method ?? throw new ArgumentNullException(nameof(method));
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpContent"></param>
        /// <param name="dispose"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlPost(string uri, HttpContent httpContent, bool dispose = true)
        {
            return WithUrl(new Uri(uri), HttpMethod.Post).WithBody(httpContent, dispose);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpContent"></param>
        /// <param name="dispose"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlPost(Uri uri, HttpContent httpContent, bool dispose = true)
        {
            return WithUrl(uri, HttpMethod.Post).WithBody(httpContent, dispose);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpContent"></param>
        /// <param name="dispose"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlPost(UriBuilder uri, HttpContent httpContent, bool dispose = true)
        {
            return WithUrl((Uri)uri, HttpMethod.Post).WithBody(httpContent, dispose);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlPostJson(string uri, object obj)
        {
            return WithUrl(new Uri(uri), HttpMethod.Post).WithJsonBody(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlPostJson(Uri uri, object obj)
        {
            return WithUrl(uri, HttpMethod.Post).WithJsonBody(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlPostJson(UriBuilder uri, object obj)
        {
            return WithUrl((Uri)uri, HttpMethod.Post).WithJsonBody(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public RequestBuilder WithCancellationToken(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="dispose"></param>
        /// <returns></returns>
        public RequestBuilder WithBody(HttpContent httpContent, bool dispose = true)
        {
            this.httpContent = httpContent ?? throw new ArgumentNullException(nameof(httpContent));
            this.httpContentDispose = dispose;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public RequestBuilder WithJsonBody(object obj)
        {
            return WithJsonBody(obj, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public RequestBuilder WithJsonBody(object obj, Encoding encoding)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            this.httpContent = new StringContent(JsonConvert.SerializeObject(obj), encoding, "application/json");
            this.httpContentDispose = true;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RequestBuilder WithHeader(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            headers[key] = value;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<HttpResponseMessage> ExecuteAsync()
        {
            if (method == null || uri == null) throw new InvalidOperationException($"method or uri is null");
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, uri);
            foreach (var header in headers) httpRequestMessage.Headers.Add(header.Key, header.Value);
            if (httpRequestMessage.Headers.Accept.Count == 0 && HttpClient.DefaultRequestHeaders.Accept.Count == 0) 
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (httpContent != null) httpRequestMessage.Content = httpContent;
            HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
            if (httpContentDispose) httpContent?.Dispose();
            return httpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public Task<TResult> ExecuteAsync<TResult>() where TResult : class
        {
            return ExecuteAsync<TResult, string>();
        }

        static readonly Type typeString = typeof(string);
        static readonly Type typeBuffer = typeof(byte[]);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task<TResult> ExecuteAsync<TResult, TException>() where TResult : class where TException : class
        {
            using HttpResponseMessage rep = await ExecuteAsync();
            if (rep.IsSuccessStatusCode)
            {
                if (typeof(TResult).Equals(typeBuffer))
                {
                    return (await rep.Content.ReadAsByteArrayAsync()) as TResult;
                }
                else
                {
                    string content_res = await rep.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (typeof(TResult).Equals(typeString))
                    {
                        return content_res as TResult;
                    }
                    else
                        return JsonConvert.DeserializeObject<TResult>(content_res);
                }
            }
            else
            {
                string content_res = await rep.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (typeof(TException).Equals(typeString))
                {
                    throw new ApiException<string>()
                    {
                        Body = content_res,
                        StatusCode = rep.StatusCode
                    };
                }
                else throw new ApiException<TException>()
                {
                    Body = JsonConvert.DeserializeObject<TException>(content_res),
                    StatusCode = rep.StatusCode
                };
            }
        }
    }
}