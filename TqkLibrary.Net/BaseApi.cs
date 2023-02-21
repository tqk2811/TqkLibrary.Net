using System;
using System.Net;
using System.Net.Http;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(ApiException)}: {StatusCode}";
        }
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
        /// <summary>
        /// 
        /// </summary>
        public readonly string ApiKey;

        /// <summary>
        /// 
        /// </summary>
        internal protected readonly HttpClient httpClient;

        /// <summary>
        /// 
        /// </summary>
        protected BaseApi()
        {
            this.httpClient = new HttpClient(NetSingleton.HttpClientHandler, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        protected BaseApi(string apiKey) : this()
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            this.ApiKey = apiKey;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApi(string apiKey, HttpMessageHandler httpMessageHandler)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            this.httpClient = new HttpClient(httpMessageHandler ?? throw new ArgumentNullException(nameof(httpClient)), false);
            this.ApiKey = apiKey;
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
    }
}