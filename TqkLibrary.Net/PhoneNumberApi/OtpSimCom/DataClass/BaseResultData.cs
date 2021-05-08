using Newtonsoft.Json;

namespace TqkLibrary.Net.PhoneNumberApi.OtpSimCom
{
  public class BaseResult<T>
  {
    [JsonProperty("status_code")]
    public StatusCode StatusCode { get; set; }

    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    public T Data { get; set; }
  }
}