using Newtonsoft.Json;
using System;

namespace TqkLibrary.Net.ProxysApi.TinsoftProxyCom
{
  public class KeyInfo : BaseResult
  {
    [JsonProperty("date_expired")]
    public DateTime DateExpired { get; set; }

    [JsonProperty("isVip")]
    public int IsVip { get; set; }
  }
}