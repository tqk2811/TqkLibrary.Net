using Newtonsoft.Json;

namespace TqkLibrary.Net.PhoneNumberApi.OtpSimCom
{
  public class DataService : DataNetwork
  {
    [JsonProperty("price")]
    public double? Price { get; set; }
  }
}