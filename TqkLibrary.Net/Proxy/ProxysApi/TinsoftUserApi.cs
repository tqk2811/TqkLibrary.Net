using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxy.ProxysApi
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TinsoftProxyUserInfo : TinsoftProxyBaseResult
    {
        [JsonProperty("max_key")]
        public int MaxKey { get; set; }

        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("paid")]
        public double Paid { get; set; }

        [JsonProperty("wallet_key")]
        public string WalletKey { get; set; }
    }
    public class TinsoftProxyUserKeyInfo : TinsoftProxyBaseResult
    {
        [JsonProperty("data")]
        public List<TinsoftProxyUserKeyDataInfo> Data { get; set; }
    }
    public class TinsoftProxyOrderResult : TinsoftProxyBaseResult
    {
        [JsonProperty("data")]
        public List<TinsoftProxyTinsoftOrderDataResult> Data { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// 
    /// </summary>
    public class TinsoftUserApi : BaseApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserApiKey"></param>
        public TinsoftUserApi(string UserApiKey) : base(UserApiKey)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TinsoftProxyUserKeyInfo> GetUserKeys(CancellationToken cancellationToken = default)
          => RequestGetAsync<TinsoftProxyUserKeyInfo>($"{TinsoftProxyApi.EndPoint}/getUserKeys.php?key={ApiKey}", cancellationToken: cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TinsoftProxyUserInfo> GetUserInfo(CancellationToken cancellationToken = default)
          => RequestGetAsync<TinsoftProxyUserInfo>($"{TinsoftProxyApi.EndPoint}/getUserInfo.php?key={ApiKey}", cancellationToken: cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="dateTime"></param>
        /// <param name="tinsoftVip"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TinsoftProxyOrderResult> OrderKeys(int quantity, DateTime dateTime, TinsoftProxyVip tinsoftVip, CancellationToken cancellationToken = default)
          => RequestGetAsync<TinsoftProxyOrderResult>(
              $"{TinsoftProxyApi.EndPoint}/orderKeys.php?key={ApiKey}&quantity={quantity}&days={dateTime:dd-MM-yyyy HH:mm:ss}&vip={(int)tinsoftVip}",
              cancellationToken: cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="proxyKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TinsoftProxyBaseResult> ExtendKey(DateTime dateTime, string proxyKey, CancellationToken cancellationToken = default)
          => RequestGetAsync<TinsoftProxyBaseResult>(
              $"{TinsoftProxyApi.EndPoint}/extendKey.php?key={ApiKey}&days={dateTime:dd-MM-yyyy HH:mm:ss}&proxy_key={proxyKey}",
              cancellationToken: cancellationToken);
    }
}