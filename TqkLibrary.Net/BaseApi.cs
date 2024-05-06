using System;
using System.Net;
using System.Net.Http;
namespace TqkLibrary.Net
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseApi : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly string? ApiKey;

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
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApi(string apiKey) : this()
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            this.ApiKey = apiKey;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="httpMessageHandler"></param>
        /// <param name="disposeHandler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApi(string apiKey, HttpMessageHandler httpMessageHandler, bool disposeHandler = false)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            this.httpClient = new HttpClient(httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler)), disposeHandler);
            this.ApiKey = apiKey;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpMessageHandler"></param>
        /// <param name="disposeHandler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected BaseApi(HttpMessageHandler httpMessageHandler, bool disposeHandler = false)
        {
            this.httpClient = new HttpClient(httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler)), disposeHandler);
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

        protected virtual void Dispose(bool disposing)
        {
            httpClient.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual RequestBuilder Build() => new RequestBuilder(this);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                return base.ToString();
            }
            else
            {
                return $"{this.GetType().Name}({ApiKey})";
            }
        }
    }
}