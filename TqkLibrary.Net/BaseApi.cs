using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
    public class ApiException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiException<T> : ApiException
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiException()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public T Body { get; internal set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseApi : IDisposable
    {
        static readonly Type typeString = typeof(string);
        static readonly Type typeBuffer = typeof(byte[]);

        /// <summary>
        /// 
        /// </summary>
        protected readonly string ApiKey;

        /// <summary>
        /// 
        /// </summary>
        internal protected readonly HttpClient httpClient;
        /// <summary>
        /// 
        /// </summary>
        internal protected readonly HttpClientHandler httpClientHandler;
        /// <summary>
        /// 
        /// </summary>
        protected BaseApi(HttpClientHandler httpClientHandler = null)
        {
            if(httpClientHandler == null)
            {
                httpClientHandler = new HttpClientHandler();
                httpClientHandler.CookieContainer = new CookieContainer();
                httpClientHandler.UseCookies = true;
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                httpClientHandler.AllowAutoRedirect = true;
            }
            httpClient = new HttpClient(httpClientHandler, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApi(string ApiKey, HttpClientHandler httpClientHandler = null) : this(httpClientHandler)
        {
            if (string.IsNullOrEmpty(ApiKey)) throw new ArgumentNullException(nameof(ApiKey));
            this.ApiKey = ApiKey;
        }
        /// <summary>
        /// 
        /// </summary>
        ~BaseApi()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            httpClient.Dispose();
        }





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected RequestBuilder Build() => new RequestBuilder(this);




        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected Task<TResult> RequestGetAsync<TResult>(
          string url,
          Dictionary<string, string> headers = null,
          CancellationToken cancellationToken = default)
          where TResult : class
          => RequestAsync<TResult, string>(HttpMethod.Get, new Uri(url), headers, cancellationToken: cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected Task<TResult> RequestGetAsync<TResult>(
         Uri uri,
         Dictionary<string, string> headers = null,
         CancellationToken cancellationToken = default)
         where TResult : class
         => RequestAsync<TResult, string>(HttpMethod.Get, uri, headers, cancellationToken: cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected Task<TResult> RequestGetAsync<TResult, TException>(
          string url,
          Dictionary<string, string> headers = null,
          CancellationToken cancellationToken = default)
          where TResult : class
          where TException : class
          => RequestAsync<TResult, TException>(HttpMethod.Get, new Uri(url), headers, cancellationToken: cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected Task<TResult> RequestGetAsync<TResult, TException>(
          Uri uri,
          Dictionary<string, string> headers = null,
          CancellationToken cancellationToken = default)
          where TResult : class
          where TException : class
          => RequestAsync<TResult, TException>(HttpMethod.Get, uri, headers, cancellationToken: cancellationToken);




        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected Task<TResult> RequestPostAsync<TResult>(
          string url,
          Dictionary<string, string> headers = null,
          HttpContent httpContent = null,
          CancellationToken cancellationToken = default)
          where TResult : class
          => RequestAsync<TResult, string>(HttpMethod.Post, new Uri(url), headers, httpContent, cancellationToken: cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected Task<TResult> RequestPostAsync<TResult>(
         Uri uri,
         Dictionary<string, string> headers = null,
         HttpContent httpContent = null,
          CancellationToken cancellationToken = default)
         where TResult : class
         => RequestAsync<TResult, string>(HttpMethod.Post, uri, headers, httpContent, cancellationToken: cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected Task<TResult> RequestPostAsync<TResult, TException>(
          string url,
          Dictionary<string, string> headers = null,
          HttpContent httpContent = null,
          CancellationToken cancellationToken = default)
          where TResult : class
          where TException : class
          => RequestAsync<TResult, TException>(HttpMethod.Post, new Uri(url), headers, httpContent, cancellationToken: cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected Task<TResult> RequestPostAsync<TResult, TException>(
         Uri uri,
         Dictionary<string, string> headers = null,
         HttpContent httpContent = null,
         CancellationToken cancellationToken = default)
         where TResult : class
         where TException : class
         => RequestAsync<TResult, TException>(HttpMethod.Post, uri, headers, httpContent, cancellationToken: cancellationToken);


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        protected async Task<TResult> RequestAsync<TResult, TException>(
          HttpMethod method,
          Uri uri,
          Dictionary<string, string> headers = null,
          HttpContent httpContent = null,
          CancellationToken cancellationToken = default)
          where TResult : class
          where TException : class
        {
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, uri);
            if (headers != null) foreach (var pair in headers) httpRequestMessage.Headers.Add(pair.Key, pair.Value);
            if (httpRequestMessage.Headers.Accept.Count == 0) httpRequestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            if (httpContent != null) httpRequestMessage.Content = httpContent;
            return await RequestAsync<TResult, TException>(httpRequestMessage, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="req"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        protected async Task<TResult> RequestAsync<TResult, TException>(HttpRequestMessage req, CancellationToken cancellationToken = default)
            where TResult : class
            where TException : class
        {
            using HttpResponseMessage rep = await httpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
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