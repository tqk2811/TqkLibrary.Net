using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace TqkLibrary.Net.ProxysApi.TtProxyCom
{
  public class TtProxyApi : BaseApi
  {
    const string EndPoint = "https://api.ttproxy.com/v1/";
    readonly string secret;
    public TtProxyApi(string license, string secret) : base(license)
    {
      if (string.IsNullOrEmpty(secret)) throw new ArgumentNullException(nameof(secret));
      this.secret = secret;
    }

    string GenerateParameters()
    {
      long unix = DateTimeOffset.Now.ToUnixTimeSeconds();
      string sign = ApiKey + unix + secret;
      string sign_md5;
      using (MD5 md5 = MD5.Create())
      {
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(sign);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        sign_md5 = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
      }
      return $"license={ApiKey}&time={unix}&sign={sign_md5}";
    }

    public Task<TtProxyResult<ObtainResult>> Obtain()
      => RequestGet<TtProxyResult<ObtainResult>>($"{EndPoint}obtain?{GenerateParameters()}");

    public Task<TtProxyResult<ObtainResult>> Obtain(int count)
      => RequestGet<TtProxyResult<ObtainResult>>($"{EndPoint}obtain?{GenerateParameters()}&cnt={count}");

    public Task<TtProxyResult<List<string>>> WhiteListQuery()
      => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/query?{GenerateParameters()}");

    public Task<TtProxyResult<List<string>>> WhiteListExists(string ip)
      => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/exists?{GenerateParameters()}&ip={ip}");

    public Task<TtProxyResult<List<string>>> WhiteListAdd(string ip)
      => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/add?{GenerateParameters()}&ip={ip}");

    public Task<TtProxyResult<List<string>>> WhiteListDelete(string ip)
      => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/del?{GenerateParameters()}&ip={ip}");

    public Task<TtProxyResult<List<string>>> WhiteListClear(string ip)
      => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/clear?{GenerateParameters()}&ip={ip}");

    public Task<TtProxyResult<List<string>>> SubLicenseList()
     => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}subLicense/list?{GenerateParameters()}");

    public Task<TtProxyResult<SubLicenseResult>> SubLicenseList(int page)
      => RequestGet<TtProxyResult<SubLicenseResult>>($"{EndPoint}subLicense/list?{GenerateParameters()}&page={page}");

    public Task<TtProxyResult<SubLicenseCreateResult>> SubLicenseCreate()
      => RequestGet<TtProxyResult<SubLicenseCreateResult>>($"{EndPoint}subLicense/create?{GenerateParameters()}");

    public Task<TtProxyResult<SubLicenseCreateResult>> SubLicenseCreate(int traffic)
      => RequestPost<TtProxyResult<SubLicenseCreateResult>>($"{EndPoint}subLicense/create?{GenerateParameters()}", new StringContent($"traffic={traffic}"));

    public Task<TtProxyResult<SubLicenseRenewResult>> SubLicenseRenew()
      => RequestGet<TtProxyResult<SubLicenseRenewResult>>($"{EndPoint}subLicense/renew?{GenerateParameters()}");

    public Task<TtProxyResult<SubLicenseRenewResult>> SubLicenseRenew(int traffic, string subLicenseKey)
      => RequestPost<TtProxyResult<SubLicenseRenewResult>>($"{EndPoint}subLicense/renew?{GenerateParameters()}", new StringContent($"traffic={traffic}&key={subLicenseKey}"));

    public Task<TtProxyResult<SubLicenseRenewResult>> SubLicenseReclaim()
      => RequestGet<TtProxyResult<SubLicenseRenewResult>>($"{EndPoint}subLicense/reclaim?{GenerateParameters()}");

    public Task<TtProxyResult<SubLicenseRenewResult>> SubLicenseReclaim(string key)
      => RequestPost<TtProxyResult<SubLicenseRenewResult>>($"{EndPoint}subLicense/reclaim?{GenerateParameters()}", new StringContent($"key={key}"));
  }
}
