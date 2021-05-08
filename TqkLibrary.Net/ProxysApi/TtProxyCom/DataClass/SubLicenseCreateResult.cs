using System.Collections.Generic;

namespace TqkLibrary.Net.ProxysApi.TtProxyCom
{
  public class SubLicenseCreateResult
  {
    public List<SubLicenseLicenseResult> licenses { get; set; }
    public long? trafficLeft { get; set; }
  }
}
