using Newtonsoft.Json;

namespace TqkLibrary.Net.PhoneNumberApi.SimThueCom
{
  public class RequestResult : BalanceResult
  {
    [JsonProperty("id")]
    public string Id { get; set; }
  }
}