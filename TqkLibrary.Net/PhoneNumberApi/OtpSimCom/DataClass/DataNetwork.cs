using Newtonsoft.Json;

namespace TqkLibrary.Net.PhoneNumberApi.OtpSimCom
{
  public class DataNetwork
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
  }
}