using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.ProxysApi
{
  

  public class TinsoftProxyUserKeyDataInfo : TinsoftProxyBaseResult
  {
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("date_expired")]
    public DateTime DateExpired { get; set; }

    [JsonProperty("isVip")]
    public int IsVip { get; set; }
  }
 
  public enum TinsoftProxyVip
  {
    /// <summary>
    /// key thường
    /// </summary>
    Nomal = 0,

    /// <summary>
    /// key vip ko timeout
    /// </summary>
    NonTimeout = 1,

    /// <summary>
    /// key vip dùng nhanh (1phút được đổi ip)
    /// </summary>
    Fast = 2
  }
  public class TinsoftProxyProxyResult : TinsoftProxyBaseResult
  {
    [JsonProperty("proxy")]
    public string Proxy { get; set; }

    [JsonProperty("next_change")]
    public int? NextChange { get; set; }

    [JsonProperty("timeout")]
    public int? Timeout { get; set; }
  }
  

  public class TinsoftProxyTinsoftOrderDataResult : TinsoftProxyBaseResult
  {
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("date_expired")]
    public DateTime DateExpired { get; set; }

    [JsonProperty("isVip")]
    public int? IsVip { get; set; }
  }
  public class TinsoftProxyLocationResult : TinsoftProxyBaseResult
  {
    public List<TinsoftProxyLocation> data { get; set; }
  }
  public class TinsoftProxyLocation
  {
    public int? location { get; set; }
    public string name { get; set; }

    public override string ToString() => $"location: {location}, name: {name}";
  }
  public class TinsoftProxyKeyInfo : TinsoftProxyBaseResult
  {
    [JsonProperty("date_expired")]
    public DateTime DateExpired { get; set; }

    [JsonProperty("isVip")]
    public int IsVip { get; set; }
  }
  public class TinsoftProxyBaseResult
  {
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
  }
  /// <summary>
  /// http://proxy.tinsoftsv.com/api/document_vi.php
  /// </summary>
  public sealed class TinsoftProxyApi : BaseApi
  {
    internal const string EndPoint = "http://proxy.tinsoftsv.com/api";

    public TinsoftProxyApi(string ApiKey, CancellationToken cancellationToken = default) : base(ApiKey,cancellationToken)
    {
    }

    public Task<TinsoftProxyProxyResult> ChangeProxy(int location = 0)
      => RequestGet<TinsoftProxyProxyResult>(string.Format(EndPoint + "/changeProxy.php?key={0}&location={1}", ApiKey, location));

    public Task<TinsoftProxyKeyInfo> GetKeyInfo()
      => RequestGet<TinsoftProxyKeyInfo>(string.Format(EndPoint + "/getKeyInfo.php?key={0}", ApiKey));

    public Task<TinsoftProxyKeyInfo> DeleteKey()
      => RequestGet<TinsoftProxyKeyInfo>(string.Format(EndPoint + "/deleteKey.php?key={0}", ApiKey));

    public Task<TinsoftProxyLocationResult> GetLocations()
      => RequestGet<TinsoftProxyLocationResult>(EndPoint + "/getLocations.php");
  }
}