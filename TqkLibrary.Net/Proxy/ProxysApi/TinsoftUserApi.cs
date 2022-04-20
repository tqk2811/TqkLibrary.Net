using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxy.ProxysApi
{
  public class TinsoftProxyUserInfo : TinsoftProxyBaseResult
  {
    [JsonProperty("max_key")]
    public int? MaxKey { get; set; }

    [JsonProperty("balance")]
    public double? Balance { get; set; }

    [JsonProperty("paid")]
    public double? Paid { get; set; }

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
  public class TinsoftUserApi : BaseApi
  {
    public TinsoftUserApi(string UserApiKey, CancellationToken cancellationToken = default) : base(UserApiKey,cancellationToken)
    {
    }

    public Task<TinsoftProxyUserKeyInfo> GetUserKeys()
      => RequestGet<TinsoftProxyUserKeyInfo>(string.Format(TinsoftProxyApi.EndPoint + "/getUserKeys.php?key={0}", ApiKey));

    public Task<TinsoftProxyUserInfo> GetUserInfo()
      => RequestGet<TinsoftProxyUserInfo>(string.Format(TinsoftProxyApi.EndPoint + "/getUserInfo.php?key={0}", ApiKey));

    public Task<TinsoftProxyOrderResult> OrderKeys(int quantity, DateTime dateTime, TinsoftProxyVip tinsoftVip)
      => RequestGet<TinsoftProxyOrderResult>(string.Format(
        TinsoftProxyApi.EndPoint + "/orderKeys.php?key={0}&quantity={1}&days={2}&vip={3}",
        ApiKey,
        quantity,
        dateTime.ToString("dd-MM-yyyy HH:mm:ss"),
        (int)tinsoftVip));

    public Task<TinsoftProxyBaseResult> ExtendKey(DateTime dateTime, string proxyKey)
      => RequestGet<TinsoftProxyBaseResult>(string.Format(
        TinsoftProxyApi.EndPoint + "/extendKey.php?key={0}&days={1}&proxy_key={2}",
        ApiKey,
        dateTime.ToString("dd-MM-yyyy HH:mm:ss"),
        proxyKey));
  }
}