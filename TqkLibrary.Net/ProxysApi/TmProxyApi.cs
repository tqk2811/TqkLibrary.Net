using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.ProxysApi
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
    public int? next_request { get; set; }
    public string expired_at { get; set; }
  }
  public class TmProxyApi : BaseApi
  {
    const string EndPoint = "https://tmproxy.com/api/proxy/";
    public TmProxyApi(string ApiKey,CancellationToken cancellationToken = default) : base(ApiKey, cancellationToken)
    {

    }

    public Task<TMProxyResponse<TMProxyStatResponse>> Stats()
      => RequestPost<TMProxyResponse<TMProxyStatResponse>>(
        $"{EndPoint}stats",
        null,
        new StringContent(JsonConvert.SerializeObject(new { api_key = ApiKey })));

    public Task<TMProxyResponse<TMProxyProxyResponse>> GetCurrentProxy()
     => RequestPost<TMProxyResponse<TMProxyProxyResponse>>(
       $"{EndPoint}get-current-proxy",
       null, 
       new StringContent(JsonConvert.SerializeObject(new { api_key = ApiKey })));

    public Task<TMProxyResponse<TMProxyProxyResponse>> GetNewProxy(int id_location = 0)
     => RequestPost<TMProxyResponse<TMProxyProxyResponse>>(
       $"{EndPoint}get-new-proxy",
       null, 
       new StringContent(JsonConvert.SerializeObject(new { api_key = ApiKey, id_location = id_location })));
  }
}
