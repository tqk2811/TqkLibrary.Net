using Newtonsoft.Json;

namespace TqkLibrary.Net.PhoneNumberApi.OtpSimCom
{
  public class PhoneRequestResult
  {
    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }

    [JsonProperty("network")]
    public int NetWork { get; set; }

    [JsonProperty("session")]
    public string Session { get; set; }
  }
}