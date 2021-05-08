using System.Threading.Tasks;

namespace TqkLibrary.Net.ProxysApi.TinsoftProxyCom
{
  /// <summary>
  /// http://proxy.tinsoftsv.com/api/document_vi.php
  /// </summary>
  public sealed class TinsoftProxyApi : BaseApi
  {
    internal const string EndPoint = "http://proxy.tinsoftsv.com/api";

    public TinsoftProxyApi(string ApiKey) : base(ApiKey)
    {
    }

    public Task<ProxyResult> ChangeProxy(int location = 0)
      => RequestGet<ProxyResult>(string.Format(EndPoint + "/changeProxy.php?key={0}&location={1}", ApiKey, location));

    public Task<KeyInfo> GetKeyInfo()
      => RequestGet<KeyInfo>(string.Format(EndPoint + "/getKeyInfo.php?key={0}", ApiKey));

    public Task<KeyInfo> DeleteKey()
      => RequestGet<KeyInfo>(string.Format(EndPoint + "/deleteKey.php?key={0}", ApiKey));

    public Task<LocationResult> GetLocations()
      => RequestGet<LocationResult>(EndPoint + "/getLocations.php");
  }
}