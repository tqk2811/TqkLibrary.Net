using Newtonsoft.Json;

namespace TqkLibrary.Net.ProxysApi.TinsoftProxyCom
{
  public class BaseResult
  {
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
  }
}