namespace TqkLibrary.Net.ProxysApi.TmProxyCom
{
  public class TMProxyProxyResponse
  {
    public string ip_allow { get; set; }
    public string location_name { get; set; }
    public string socks5 { get; set; }
    public string https { get; set; }
    public int? timeout { get; set; }
    public int? next_request { get; set; }
    public string expired_at { get; set; }
  }
}
