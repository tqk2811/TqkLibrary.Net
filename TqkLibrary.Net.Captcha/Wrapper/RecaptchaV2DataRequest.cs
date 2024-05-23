using Newtonsoft.Json;
using System;

namespace TqkLibrary.Net.Captcha.Wrapper
{
    public class RecaptchaV2DataRequest
    {
        public RecaptchaV2DataRequest(
            [JsonProperty(nameof(DataSiteKey))] string dataSiteKey,
            [JsonProperty(nameof(PageUrl))] string pageUrl
            )
        {
            if (string.IsNullOrWhiteSpace(dataSiteKey)) throw new ArgumentNullException(nameof(dataSiteKey));
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException(nameof(pageUrl));
            this.DataSiteKey = dataSiteKey;
            this.PageUrl = pageUrl;
        }
        public string DataSiteKey { get; }
        public string PageUrl { get; }


        public string? Proxy { get; set; }
        public string? ProxyType { get; set; }
        public string? UserAgent { get; set; }
        public string? DataS { get; set; }
        public string? Domain { get; set; }
        public bool? IsInvisible { get; set; }
        public bool? IsEnterprise { get; set; }
        public string? Cookies { get; set; }
    }
}
