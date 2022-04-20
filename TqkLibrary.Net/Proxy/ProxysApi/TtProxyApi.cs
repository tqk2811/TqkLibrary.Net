using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxy.ProxysApi
{

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TtProxyObtainResult
    {
        [JsonProperty("todayObtain")]
        public int? TodayObtain { get; set; }

        [JsonProperty("ipLeft")]
        public int? IpLeft { get; set; }

        [JsonProperty("trafficLeft")]
        public long TrafficLeft { get; set; }

        [JsonProperty("proxies")]
        public List<string> Proxies { get; set; }
    }
    public class TtProxySubLicenseCreateResult
    {
        public List<TtProxySubLicenseLicenseResult> licenses { get; set; }
        public long? trafficLeft { get; set; }
    }
    public class TtProxySubLicenseLicenseResult
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
    public class TtProxySubLicensePageResult
    {
        public int? current { get; set; }
        public int? last { get; set; }
        public int? limit { get; set; }
        public int? total { get; set; }
    }
    public class TtProxySubLicenseRenewResult
    {
        public TtProxySubLicenseLicenseResult license { get; set; }
        public long? trafficLeft { get; set; }
    }
    public class TtProxySubLicenseResult
    {
        [JsonProperty("page")]
        public TtProxySubLicensePageResult Page { get; set; }

        [JsonProperty("list")]
        public List<TtProxySubLicenseLicenseResult> List { get; set; }
    }
    public enum TtProxyErrorCode
    {
        OK = 0,
        Failed = 1000,
        UnknownError = 1001,
        ServerInternalError = 1002,
        UnsupportedOperations = 1003,
        SendMailError = 1005,
        WrongArguments = 1010,
        LackArguments = 1011,
        InvalidData = 1012,
        AlreadyCompleted = 1100,
        AlreadyExpired = 1102,
        AlreadyExisted = 1103,
        NoModification = 1104,
        TooManyItems = 1105,
        InvalidRequest = 1400,
        AuthFailed = 1401,
        Forbidden = 1403,
        NotFound = 1404,
        InvalidCaptcha = 1405,
        NoActivation = 1406,
        UserForbidden = 1407,
        WrongPassword = 1408,
        TooManyErrorsWithIncorrectPassword = 1409,
        TooManyRequests = 1429,
        AbnormalOrderStatus = 1601,
        PaymentFailure = 1610,
        AuthFailed2 = 2000,
        ReachesTheUpperLimit = 2001,
        TemporarilyExhausted = 2003,
        WrongArguments2 = 2004,
        CredentialExpires = 2005,
        Forbidden2 = 2006,
        TooManyWhitelists = 2010
    }
    public class TtProxyResult<T>
    {
        [JsonProperty("code")]
        public TtProxyErrorCode Code { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// 
    /// </summary>
    public class TtProxyApi : BaseApi
    {
        const string EndPoint = "https://api.ttproxy.com/v1/";
        readonly string secret;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="license"></param>
        /// <param name="secret"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TtProxyApi(string license, string secret, CancellationToken cancellationToken = default) : base(license, cancellationToken)
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TtProxyResult<TtProxyObtainResult>> Obtain()
          => RequestGet<TtProxyResult<TtProxyObtainResult>>($"{EndPoint}obtain?{GenerateParameters()}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Task<TtProxyResult<TtProxyObtainResult>> Obtain(int count)
          => RequestGet<TtProxyResult<TtProxyObtainResult>>($"{EndPoint}obtain?{GenerateParameters()}&cnt={count}");
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TtProxyResult<List<string>>> WhiteListQuery()
          => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/query?{GenerateParameters()}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Task<TtProxyResult<List<string>>> WhiteListExists(string ip)
          => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/exists?{GenerateParameters()}&ip={ip}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Task<TtProxyResult<List<string>>> WhiteListAdd(string ip)
          => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/add?{GenerateParameters()}&ip={ip}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Task<TtProxyResult<List<string>>> WhiteListDelete(string ip)
          => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/del?{GenerateParameters()}&ip={ip}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Task<TtProxyResult<List<string>>> WhiteListClear(string ip)
          => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}whitelist/clear?{GenerateParameters()}&ip={ip}");
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TtProxyResult<List<string>>> SubLicenseList()
         => RequestGet<TtProxyResult<List<string>>>($"{EndPoint}subLicense/list?{GenerateParameters()}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Task<TtProxyResult<TtProxySubLicenseResult>> SubLicenseList(int page)
          => RequestGet<TtProxyResult<TtProxySubLicenseResult>>($"{EndPoint}subLicense/list?{GenerateParameters()}&page={page}");
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TtProxyResult<TtProxySubLicenseCreateResult>> SubLicenseCreate()
          => RequestGet<TtProxyResult<TtProxySubLicenseCreateResult>>($"{EndPoint}subLicense/create?{GenerateParameters()}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="traffic"></param>
        /// <returns></returns>
        public Task<TtProxyResult<TtProxySubLicenseCreateResult>> SubLicenseCreate(int traffic)
          => RequestPost<TtProxyResult<TtProxySubLicenseCreateResult>>($"{EndPoint}subLicense/create?{GenerateParameters()}", null, new StringContent($"traffic={traffic}"));
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TtProxyResult<TtProxySubLicenseRenewResult>> SubLicenseRenew()
          => RequestGet<TtProxyResult<TtProxySubLicenseRenewResult>>($"{EndPoint}subLicense/renew?{GenerateParameters()}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="traffic"></param>
        /// <param name="subLicenseKey"></param>
        /// <returns></returns>
        public Task<TtProxyResult<TtProxySubLicenseRenewResult>> SubLicenseRenew(int traffic, string subLicenseKey)
          => RequestPost<TtProxyResult<TtProxySubLicenseRenewResult>>($"{EndPoint}subLicense/renew?{GenerateParameters()}", null, new StringContent($"traffic={traffic}&key={subLicenseKey}"));
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TtProxyResult<TtProxySubLicenseRenewResult>> SubLicenseReclaim()
          => RequestGet<TtProxyResult<TtProxySubLicenseRenewResult>>($"{EndPoint}subLicense/reclaim?{GenerateParameters()}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<TtProxyResult<TtProxySubLicenseRenewResult>> SubLicenseReclaim(string key)
          => RequestPost<TtProxyResult<TtProxySubLicenseRenewResult>>($"{EndPoint}subLicense/reclaim?{GenerateParameters()}", null, new StringContent($"key={key}"));

    }
}
