﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxys
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyNo1Com : BaseApi
    {
        const string EndPoint = "https://app.proxyno1.com/api";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public ProxyNo1Com(string apiKey) : base(apiKey)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ProxyNo1ComResponse<ProxyNo1ComKeyInfo>> AllKeys(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "all-keys", ApiKey))
            .ExecuteAsync<ProxyNo1ComResponse<ProxyNo1ComKeyInfo>>(cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<ProxyNo1ComResponse<ProxyNo1ComKeyStatus>> KeyStatus(ProxyNo1ComKeyInfo keyInfo, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "key-status", keyInfo?.GetKey() ?? throw new ArgumentNullException(nameof(keyInfo))))
            .ExecuteAsync<ProxyNo1ComResponse<ProxyNo1ComKeyStatus>>(cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<ProxyNo1ComResponse> ChangeKeyIp(ProxyNo1ComKeyInfo keyInfo, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "change-key-ip", keyInfo?.GetKey() ?? throw new ArgumentNullException(nameof(keyInfo))))
            .ExecuteAsync<ProxyNo1ComResponse>(cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<ProxyNo1ComResponse> ReNew(ProxyNo1ComKeyInfo keyInfo, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "renew", keyInfo?.GetKey() ?? throw new ArgumentNullException(nameof(keyInfo))))
            .ExecuteAsync<ProxyNo1ComResponse>(cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ProxyNo1ComResponse<List<ProxyNo1ComLocation>>> GetLocation(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "get-location"))
            .ExecuteAsync<ProxyNo1ComResponse<List<ProxyNo1ComLocation>>>(cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        public static readonly IEnumerable<string> DefaultProtocols = new List<string>()
        {
            "HTTP-1",
            "HTTP-2",
            "SOCK-1",
            "SOCK-2"
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="plan"></param>
        /// <param name="ip_rotate"></param>
        /// <param name="allowIp"></param>
        /// <param name="protocols">if null, use <see cref="DefaultProtocols"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<ProxyNo1ComResponse<ProxyNo1ComProxyInfo>> BuyKey(
            ProxyNo1ComLocation location,
            int plan = 1,
            bool ip_rotate = false,
            IPAddress allowIp = null,
            IEnumerable<string> protocols = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(location?.Location)) throw new ArgumentNullException(nameof(location));
            if (plan <= 0) throw new ArgumentException($"{nameof(plan)} need > 0");

            dynamic data = new { };
            data.token = ApiKey;
            if (protocols == null) data.protocol = string.Join(";", DefaultProtocols);
            else data.protocol = string.Join(";", protocols);
            data.location = location?.Location;
            data.plan = $"{plan}";
            if (allowIp != null) data.allowip = $"{allowIp}";
            return Build()
               .WithUrlPostJson(new UriBuilder(EndPoint, "buy-key"), data)
               .ExecuteAsync<ProxyNo1ComResponse<ProxyNo1ComProxyInfo>>(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <param name="allowIp"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<ProxyNo1ComResponse> EditKey(
            ProxyNo1ComKeyInfo keyInfo,
            IPAddress allowIp = null,
            string userName = null,
            string passWord = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(keyInfo?.Key)) throw new ArgumentNullException(nameof(keyInfo));

            if ((string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(passWord)) && allowIp == null)
            {
                throw new ArgumentException($"Need to fill in at least one of the two fields: {nameof(allowIp)} or {nameof(userName)} & {nameof(passWord)}");
            }


            dynamic data = new { };
            data.token = ApiKey;
            data.key = keyInfo.Key;

            if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(passWord))
            {
                data.authen = $"{userName}:{passWord}";
            }
            if (allowIp != null) data.allowip = allowIp.ToString();
            return Build()
               .WithUrlPostJson(new UriBuilder(EndPoint, "edit-key"), data)
               .ExecuteAsync<ProxyNo1ComResponse>(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<ProxyNo1ComResponse> RebootKey(ProxyNo1ComKeyInfo keyInfo, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "reboot-key", keyInfo?.GetKey() ?? throw new ArgumentNullException(nameof(keyInfo))))
            .ExecuteAsync<ProxyNo1ComResponse>(cancellationToken);
    }




#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ProxyNo1ComResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess { get { return Status == 0; } }
    }
    public class ProxyNo1ComResponse<T> : ProxyNo1ComResponse
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }
    public class ProxyNo1ComKeyInfo
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("date_expired")]
        public string DateExpired { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonIgnore]
        public DateTime? GetDateExpired
        {
            get
            {
                if (DateTime.TryParseExact(
                    DateExpired,
                    "yyyy-MM-dd HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime date))
                {
                    return date;
                }
                return null;
            }
        }

        public static implicit operator ProxyNo1ComKeyInfo(string key) => new ProxyNo1ComKeyInfo() { Key = key };
        internal string GetKey()
        {
            if (string.IsNullOrWhiteSpace(Key)) throw new NullReferenceException(nameof(Key));
            return Key;
        }
    }
    public class ProxyNo1ComKeyStatus
    {
        [JsonProperty("account")]
        public object Account { get; set; }

        [JsonProperty("client_ip")]
        public object ClientIp { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("expired_at")]
        public string ExpiredAt { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }

        [JsonProperty("http")]
        public object Http { get; set; }

        [JsonProperty("sock5")]
        public object Sock5 { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
    public class ProxyNo1ComPrice
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
    public class ProxyNo1ComLocation
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


        public static implicit operator ProxyNo1ComLocation(string location) => new ProxyNo1ComLocation() { Location = location };
    }
    public class ProxyNo1ComProxyInfo
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("HTTP_IPv4")]
        public int HTTPIPv4 { get; set; }

        [JsonProperty("HTTP_IPv6")]
        public int HTTPIPv6 { get; set; }

        [JsonProperty("SocksV5_IPv4")]
        public int SocksV5IPv4 { get; set; }

        [JsonProperty("SocksV5_IPv6")]
        public int SocksV5IPv6 { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}