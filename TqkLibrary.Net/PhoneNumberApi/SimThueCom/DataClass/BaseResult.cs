using Newtonsoft.Json;

namespace TqkLibrary.Net.PhoneNumberApi.SimThueCom
{
  public abstract class BaseResult
  {
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
  }
}