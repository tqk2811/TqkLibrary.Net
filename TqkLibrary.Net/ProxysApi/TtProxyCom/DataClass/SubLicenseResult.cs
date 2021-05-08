using Newtonsoft.Json;
using System.Collections.Generic;

namespace TqkLibrary.Net.ProxysApi.TtProxyCom
{
  public class SubLicenseResult
  {
    [JsonProperty("page")]
    public SubLicensePageResult Page { get; set; }

    [JsonProperty("list")]
    public List<SubLicenseLicenseResult> List { get; set; }
  }
}
