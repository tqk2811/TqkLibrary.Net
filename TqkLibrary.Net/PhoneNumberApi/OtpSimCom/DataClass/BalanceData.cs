using Newtonsoft.Json;

namespace TqkLibrary.Net.PhoneNumberApi.OtpSimCom
{
  public class BalanceData
  {
    [JsonProperty("balance")]
    public double Balance { get; set; }
  }
}