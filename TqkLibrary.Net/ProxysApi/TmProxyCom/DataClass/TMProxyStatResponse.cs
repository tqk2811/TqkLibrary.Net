using System;

namespace TqkLibrary.Net.ProxysApi.TmProxyCom
{
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
}
