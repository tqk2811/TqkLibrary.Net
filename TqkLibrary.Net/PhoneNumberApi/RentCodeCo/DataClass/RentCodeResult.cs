using Newtonsoft.Json;

namespace TqkLibrary.Net.PhoneNumberApi.RentCodeCo
{
  public sealed class RentCodeResult
  {
    [JsonProperty("id")]
    public int? Id { get; set; }

    [JsonProperty("success")]
    public bool? Success { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    public override string ToString()
    {
      return $"Id: {Id},Success: {Success},Message: {Message}";
    }
  }
}