using Newtonsoft.Json;
using System.Collections.Generic;

namespace TqkLibrary.Net.ProxysApi.TtProxyCom
{
  public class ObtainResult
  {
    [JsonProperty("todayObtain")]
    public int? TodayObtain { get; set; }

    [JsonProperty("ipLeft")]
    public int? IpLeft { get; set; }

    [JsonProperty("trafficLeft")]
    public long TrafficLeft { get; set; }

    [JsonProperty("proxies")]
    public List<string> Proxies { get; set; }
  }
}
