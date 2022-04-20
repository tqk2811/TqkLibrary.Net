using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxy.ProxysApi
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TMProxyStatResponse
    {
        public int? id { get; set; }
        public DateTime? expired_at { get; set; }
        public string plan { get; set; }
        public int? price_per_day { get; set; }
        public int? timeout { get; set; }
        public int? base_next_request { get; set; }
        public string api_key { get; set; }
        public string note { get; set; }
        public int? max_ip_per_day { get; set; }
        public int? ip_used_today { get; set; }
    }
    public class TMProxyResponse<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
    public class TMProxyProxyResponse
    {
        public string ip_allow { get; set; }
        public string location_name { get; set; }
        public string socks5 { get; set; }
        public string https { get; set; }
        public int? timeout { get; set; }
        public int next_request { get; set; }
        public DateTime expired_at { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// 
    /// </summary>
    public class TmProxyApi : BaseApi
    {
        const string EndPoint = "https://tmproxy.com/api/proxy/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApiKey"></param>
        /// <param name="cancellationToken"></param>
        public TmProxyApi(string ApiKey, CancellationToken cancellationToken = default) : base(ApiKey, cancellationToken)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TMProxyResponse<TMProxyStatResponse>> Stats()
          => RequestPost<TMProxyResponse<TMProxyStatResponse>>(
            $"{EndPoint}stats",
            null,
            new StringContent(JsonConvert.SerializeObject(new { api_key = ApiKey })));
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TMProxyResponse<TMProxyProxyResponse>> GetCurrentProxy()
         => RequestPost<TMProxyResponse<TMProxyProxyResponse>>(
           $"{EndPoint}get-current-proxy",
           null,
           new StringContent(JsonConvert.SerializeObject(new { api_key = ApiKey })));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_location"></param>
        /// <returns></returns>
        public Task<TMProxyResponse<TMProxyProxyResponse>> GetNewProxy(int id_location = 0)
         => RequestPost<TMProxyResponse<TMProxyProxyResponse>>(
           $"{EndPoint}get-new-proxy",
           null,
           new StringContent(JsonConvert.SerializeObject(new { api_key = ApiKey, id_location = id_location })));

    }
}
