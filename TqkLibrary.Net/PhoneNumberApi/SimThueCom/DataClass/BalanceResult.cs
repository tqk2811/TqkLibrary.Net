using Newtonsoft.Json;

namespace TqkLibrary.Net.PhoneNumberApi.SimThueCom
{
  public class BalanceResult : BaseResult
  {
    [JsonProperty("balance")]
    public double? Balance { get; set; }
  }
}