using Newtonsoft.Json;

namespace TqkLibrary.Net.ProxysApi.TinsoftProxyCom
{
  public class UserInfo : BaseResult
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
}