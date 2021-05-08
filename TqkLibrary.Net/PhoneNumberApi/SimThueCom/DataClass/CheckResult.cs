using Newtonsoft.Json;
using System.Collections.Generic;

namespace TqkLibrary.Net.PhoneNumberApi.SimThueCom
{
  public class CheckResult : BaseResult
  {
    [JsonProperty("number")]
    public int? Number { get; set; }

    [JsonProperty("timeleft")]
    public int? TimeLeft { get; set; }

    /// <summary>
    /// example: sms: ["1612133550|Google|G-195829 is your Google verification code.", "1512113580|Google|G-120094 is your Google verification code."]
    /// </summary>
    [JsonProperty("sms")]
    public List<string> Sms { get; set; }
  }
}