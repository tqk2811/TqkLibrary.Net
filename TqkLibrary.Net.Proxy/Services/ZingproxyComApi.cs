using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxy.Services
{
    public class ZingproxyComApi : BaseApi
    {
        const string EndPoint = "https://api.zingproxy.com";
        public ZingproxyComApi(string apiKey) : base(apiKey)
        {
        }



        public Task<ProxyResponse> GetProxy(CancellationToken cancellationToken = default) 
            => Build()
                .WithUrlGet(new UrlBuilder(EndPoint, "get-proxy").WithParam("key", ApiKey!))
                .ExecuteAsync<ProxyResponse>(cancellationToken);


















        public class ProxyData
        {
            [JsonProperty("uId")]
            public string? UId { get; set; }

            [JsonProperty("resourceId")]
            public string? ResourceId { get; set; }

            [JsonProperty("httpProxy")]
            public string? HttpProxy { get; set; }

            [JsonProperty("socks5Proxy")]
            public string? Socks5Proxy { get; set; }

            [JsonProperty("createdAt")]
            public DateTime? CreatedAt { get; set; }

            [JsonProperty("dateEnd")]
            public DateTime? DateEnd { get; set; }

            [JsonProperty("autoRenew")]
            public bool? AutoRenew { get; set; }
        }

        public class BaseResponse
        {
            [JsonProperty("status")]
            public ResponseStatus Status { get; set; }

            [JsonProperty("error")]
            public string? Error { get; set; }
        }
        public class ProxyResponse : BaseResponse
        {
            [JsonProperty("proxy")]
            public ProxyData? Proxy { get; set; }
        }

        public enum ResponseStatus
        {
            Success,
            Error,
        }
    }
}
