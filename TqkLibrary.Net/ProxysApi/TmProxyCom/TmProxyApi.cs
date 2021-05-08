using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace TqkLibrary.Net.ProxysApi.TmProxyCom
{
  public class TmProxyApi : BaseApi
  {
    const string EndPoint = "https://tmproxy.com/api/proxy/";
    public TmProxyApi(string ApiKey) : base(ApiKey)
    {

    }

    public Task<TMProxyResponse<TMProxyStatResponse>> Stats()
      => RequestPost<TMProxyResponse<TMProxyStatResponse>>($"{EndPoint}stats", new StringContent(JsonConvert.SerializeObject(new { api_key = ApiKey })));

    public Task<TMProxyResponse<TMProxyProxyResponse>> GetCurrentProxy()
     => RequestPost<TMProxyResponse<TMProxyProxyResponse>>($"{EndPoint}get-current-proxy", new StringContent(JsonConvert.SerializeObject(new { api_key = ApiKey })));

    public Task<TMProxyResponse<TMProxyProxyResponse>> GetNewProxy(int id_location = 0)
     => RequestPost<TMProxyResponse<TMProxyProxyResponse>>($"{EndPoint}get-new-proxy", new StringContent(JsonConvert.SerializeObject(new { api_key = ApiKey, id_location = id_location })));
  }
}
