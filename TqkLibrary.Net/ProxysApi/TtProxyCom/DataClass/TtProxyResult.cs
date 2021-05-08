using Newtonsoft.Json;

namespace TqkLibrary.Net.ProxysApi.TtProxyCom
{
  public class TtProxyResult<T>
  {
    [JsonProperty("code")]
    public TtProxyErrorCode Code { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("_id")]
    public string Id { get; set; }

    [JsonProperty("data")]
    public T Data { get; set; }
  }
}
