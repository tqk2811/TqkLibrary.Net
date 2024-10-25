using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        readonly BaseApi _baseApi;
        internal RequestBuilder(BaseApi baseApi)
        {
            this._baseApi = baseApi;
            this._httpClient = baseApi.httpClient;
        }
        HttpClient _httpClient { get; set; }
        HttpContent? _httpContent = null;
        bool _httpContentDispose = true;

        Dictionary<string, string> _headers = new Dictionary<string, string>();
        HttpMethod? _method = null;
        Uri? _uri = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public RequestBuilder WithUrl(UrlBuilder uri, HttpMethod method)
        {
            return WithUrl((Uri)uri, method);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public RequestBuilder WithUrlGet(UrlBuilder uri)
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
            this._uri = uri ?? throw new ArgumentNullException(nameof(uri));
            this._method = method ?? throw new ArgumentNullException(nameof(method));
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
        public RequestBuilder WithUrlPost(UrlBuilder uri, HttpContent httpContent, bool dispose = true)
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
        public RequestBuilder WithUrlPostJson(UrlBuilder uri, object obj)
        {
            return WithUrl((Uri)uri, HttpMethod.Post).WithJsonBody(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="dispose"></param>
        /// <returns></returns>
        public RequestBuilder WithBody(HttpContent httpContent, bool dispose = true)
        {
            this._httpContent = httpContent ?? throw new ArgumentNullException(nameof(httpContent));
            this._httpContentDispose = dispose;
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
            this._httpContent = new StringContent(JsonConvert.SerializeObject(obj, _baseApi.DefaultJsonSerializerSettings), encoding, "application/json");
            this._httpContentDispose = true;
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
            _headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (_method == null || _uri == null) throw new InvalidOperationException($"method or uri is null");
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(_method, _uri);

            foreach (var header in _headers) httpRequestMessage.Headers.Add(header.Key, header.Value);

            if (httpRequestMessage.Headers.Accept.Count == 0 && _httpClient.DefaultRequestHeaders.Accept.Count == 0)
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (string.IsNullOrWhiteSpace(httpRequestMessage.Headers.Host) && string.IsNullOrWhiteSpace(_httpClient.DefaultRequestHeaders.Host))
                httpRequestMessage.Headers.Host = _uri.Host;

            if (_httpContent != null) httpRequestMessage.Content = _httpContent;
            await _baseApi.OnBeforeRequestAsync(httpRequestMessage);

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
            if (_httpContentDispose) _httpContent?.Dispose();
            return httpResponseMessage;
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
        public async Task<TResult> ExecuteAsync<TResult, TException>(CancellationToken cancellationToken = default)
            where TResult : class
            where TException : class
        {
            using HttpResponseMessage rep = await ExecuteAsync(cancellationToken);
            if (rep.IsSuccessStatusCode)
            {
                if (typeof(TResult).Equals(typeBuffer))
                {
#if NETFRAMEWORK || NETSTANDARD
                    return ((await rep.Content.ReadAsByteArrayAsync()) as TResult)!;
#else
                    return ((await rep.Content.ReadAsByteArrayAsync(cancellationToken)) as TResult)!;
#endif
                }
            }

            string? content_res = await rep.Content
#if NETFRAMEWORK || NETSTANDARD
                    .ReadAsStringAsync()
#else
                    .ReadAsStringAsync(cancellationToken)
#endif
                    .ConfigureAwait(false);

            if (rep.IsSuccessStatusCode)
            {
                if (typeof(TResult).Equals(typeString))
                {
                    return (content_res as TResult)!;
                }
                else
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<TResult>(content_res, _baseApi.DefaultJsonSerializerSettings)!;
                    }
                    catch
                    {
                        //jump to error
                    }
                }
            }

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
                Body = JsonConvert.DeserializeObject<TException>(content_res)!,
                StatusCode = rep.StatusCode
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public Task<TResult> ExecuteAsync<TResult>(CancellationToken cancellationToken = default) where TResult : class
        {
            return ExecuteAsync<TResult, string>(cancellationToken);
        }
    }
}