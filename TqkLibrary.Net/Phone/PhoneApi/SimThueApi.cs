using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Phone.PhoneApi
{
    public class SimThueServicesResult : SimThueBaseResult
    {
        [JsonProperty("services")]
        public List<SimThueServiceResult> Services { get; set; }
    }

    public class SimThueServiceResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }
    }
    public class SimThueRequestResult : SimThueBalanceResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    public class SimThueCheckResult : SimThueBaseResult
    {
        [JsonProperty("number")]
        public int? Number { get; set; }

        [JsonProperty("timeleft")]
        public int? TimeLeft { get; set; }

        /// <summary>
        /// example: sms: ["1612133550|Google|G-195829 is your Google verification code.", "1512113580|Google|G-120094 is your Google verification code."]
        /// </summary>
        [JsonProperty("sms")]
        public List<string> Sms { get; set; }
    }
    public abstract class SimThueBaseResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
    public class SimThueBalanceResult : SimThueBaseResult
    {
        [JsonProperty("balance")]
        public double? Balance { get; set; }
    }
    /// <summary>
    /// https://simthue.com/vi/api/index
    /// </summary>
    public sealed class SimThueApi : BaseApi
    {
        private const string EndPoint = "http://api.simthue.com";

        public SimThueApi(string ApiKey) : base(ApiKey, NetSingleton.httpClient)
        {
        }

        public Task<SimThueBalanceResult> GetBalance()
          => RequestGetAsync<SimThueBalanceResult>(string.Format(EndPoint + "/balance?key={0}", ApiKey));

        public Task<SimThueServicesResult> GetAvailableServices()
          => RequestGetAsync<SimThueServicesResult>(string.Format(EndPoint + "/service?key={0}", ApiKey));

        public Task<SimThueRequestResult> CreateRequest(SimThueServiceResult serviceResult)
        {
            if (null == serviceResult) throw new ArgumentNullException(nameof(serviceResult));
            return RequestGetAsync<SimThueRequestResult>(string.Format(EndPoint + "/create?key={0}&service_id={1}", ApiKey, serviceResult.Id));
        }

        public Task<SimThueCheckResult> CheckRequest(SimThueRequestResult createResult)
        {
            if (null == createResult) throw new ArgumentNullException(nameof(createResult));
            return RequestGetAsync<SimThueCheckResult>(string.Format(EndPoint + "/check?key={0}&id={1}", ApiKey, createResult.Id));
        }

        public Task<SimThueRequestResult> CancelRequest(SimThueRequestResult createResult)
        {
            if (null == createResult) throw new ArgumentNullException(nameof(createResult));
            return RequestGetAsync<SimThueRequestResult>(string.Format(EndPoint + "/cancel?key={0}&id={1}", ApiKey, createResult.Id));
        }

        public Task<SimThueRequestResult> FinishRequest(SimThueRequestResult createResult)
        {
            if (null == createResult) throw new ArgumentNullException(nameof(createResult));
            return RequestGetAsync<SimThueRequestResult>(string.Format(EndPoint + "/cancel?key={0}&id={1}", ApiKey, createResult.Id));
        }
    }
}