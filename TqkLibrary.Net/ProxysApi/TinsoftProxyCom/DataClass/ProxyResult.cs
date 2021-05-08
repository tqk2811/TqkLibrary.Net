using Newtonsoft.Json;

namespace TqkLibrary.Net.ProxysApi.TinsoftProxyCom
{
  public class ProxyResult : BaseResult
  {
    [JsonProperty("proxy")]
    public string Proxy { get; set; }

    [JsonProperty("next_change")]
    public int? NextChange { get; set; }

    [JsonProperty("timeout")]
    public int? Timeout { get; set; }
  }
}