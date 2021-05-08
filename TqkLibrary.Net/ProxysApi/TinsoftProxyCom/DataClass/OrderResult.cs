using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TqkLibrary.Net.ProxysApi.TinsoftProxyCom
{
  public class OrderResult : BaseResult
  {
    [JsonProperty("data")]
    public List<TinsoftOrderDataResult> Data { get; set; }
  }

  public class TinsoftOrderDataResult : BaseResult
  {
    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("date_expired")]
    public DateTime DateExpired { get; set; }

    [JsonProperty("isVip")]
    public int? IsVip { get; set; }
  }
}