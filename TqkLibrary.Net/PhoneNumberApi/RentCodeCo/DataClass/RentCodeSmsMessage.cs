using Newtonsoft.Json;
using System;

namespace TqkLibrary.Net.PhoneNumberApi.RentCodeCo
{
  public sealed class RentCodeSmsMessage
  {
    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("sender")]
    public string Sender { get; set; }

    [JsonProperty("time")]
    public DateTime Time { get; set; }
  }
}