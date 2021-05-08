using System;

namespace TqkLibrary.Net.ProxysApi.TtProxyCom
{
  public class SubLicenseLicenseResult
  {
    public int? id { get; set; }
    public string key { get; set; }
    public string secret { get; set; }
    public int? obtainLimit { get; set; }
    public long? trafficLeft { get; set; }
    public int? ipDuration { get; set; }
    public string remark { get; set; }
    public long? totalTraffic { get; set; }
    public int? ipUsed { get; set; }
    public DateTime? updated { get; set; }
    public DateTime? created { get; set; }
  }
}
