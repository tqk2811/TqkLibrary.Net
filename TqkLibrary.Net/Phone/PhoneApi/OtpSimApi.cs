using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TqkLibrary.Net.Phone.PhoneApi
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum OtpSimStatusCode
    {
        Success = 200,
        NotEnoughWallet = 201,
        ApplicationNotFoundOrPaused = 202,
        PhoneNumberIsTemporarilyRunningOut = 203,
        UnAuthenticated = 401,
        Error = -1
    }
    public class OtpSimRefundData
    {
        public double Refund { get; set; }
    }
    public class OtpSimPhoneRequestResult
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("network")]
        public int NetWork { get; set; }

        [JsonProperty("session")]
        public string Session { get; set; }
    }
    public class OtpSimPhoneData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("service_id")]
        public int ServiceId { get; set; }

        [JsonProperty("service_name")]
        public string ServiceName { get; set; }

        [JsonProperty("status")]
        public OtpSimPhoneDataStatus Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("done_at")]
        public DateTime DoneAt { get; set; }

        [JsonProperty("messages")]
        public List<OtpSimPhoneDataMessage> Messages { get; set; }
    }

    public enum OtpSimPhoneDataStatus
    {
        Waiting = 1,
        Completed = 0,
        Expired = 2
    }

    public class OtpSimPhoneDataMessage
    {
        [JsonProperty("sms_from")]
        public string SmsFrom { get; set; }

        [JsonProperty("sms_content")]
        public string SmsContent { get; set; }

        [JsonProperty("is_audio")]
        public bool IsAudio { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("otp")]
        public string Otp { get; set; }

        [JsonProperty("audio_file")]
        public string AudioFile { get; set; }

        [JsonProperty("audio_content")]
        public string AudioContent { get; set; }
    }
    public class OtpSimDataService : OtpSimDataNetwork
    {
        [JsonProperty("price")]
        public double? Price { get; set; }
    }
    public class OtpSimDataNetwork
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class OtpSimBalanceData
    {
        [JsonProperty("balance")]
        public double Balance { get; set; }
    }
    public class OtpSimBaseResult<T>
    {
        [JsonProperty("status_code")]
        public OtpSimStatusCode StatusCode { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// 
    /// </summary>
    public sealed class OtpSimApi : BaseApi
    {
        private const string EndPoint = "http://otpsim.com/api";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApiKey"></param>
        public OtpSimApi(string ApiKey) : base(ApiKey, NetSingleton.httpClient)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<OtpSimBaseResult<List<OtpSimDataNetwork>>> GetNetworks(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "/networks").WithParam("token", ApiKey))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<OtpSimBaseResult<List<OtpSimDataNetwork>>>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<OtpSimBaseResult<List<OtpSimDataService>>> GetServices(CancellationToken cancellationToken = default)
             => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "/service/request").WithParam("token", ApiKey))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<OtpSimBaseResult<List<OtpSimDataService>>>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<OtpSimBaseResult<OtpSimPhoneRequestResult>> PhonesRequest(
          OtpSimDataService dataService,
          IEnumerable<OtpSimDataNetwork> dataNetworks = null,
          IEnumerable<string> prefixs = null,
          IEnumerable<string> exceptPrefixs = null,
          CancellationToken cancellationToken = default)
             => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "/phones/request")
                 .WithParam("token", ApiKey)
                 .WithParam("service", dataService.Id)
                 .WithParamIfNotNull("network", dataNetworks == null ? string.Join(",", dataNetworks.Select(x => x.Id)) : null)
                 .WithParamIfNotNull("prefix", prefixs == null ? string.Join(",", prefixs) : null)
                 .WithParamIfNotNull("exceptPrefix", exceptPrefixs == null ? string.Join(",", exceptPrefixs) : null))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<OtpSimBaseResult<OtpSimPhoneRequestResult>>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<OtpSimBaseResult<OtpSimPhoneRequestResult>> PhonesRequest(OtpSimDataService dataService, string numberBuyBack, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "/phones/request")
                 .WithParam("token", ApiKey)
                 .WithParam("service", dataService.Id)
                 .WithParam("numberBuyBack", numberBuyBack))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<OtpSimBaseResult<OtpSimPhoneRequestResult>>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<OtpSimBaseResult<OtpSimPhoneData>> GetPhoneMessage(OtpSimPhoneRequestResult phoneRequestResult, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "/sessions/", phoneRequestResult.Session)
                 .WithParam("token", ApiKey))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<OtpSimBaseResult<OtpSimPhoneData>>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<OtpSimBaseResult<OtpSimRefundData>> CancelGetPhoneMessage(OtpSimPhoneRequestResult phoneRequestResult, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "/sessions/cancel")
                .WithParam("token", ApiKey)
                .WithParam("session", phoneRequestResult.Session))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<OtpSimBaseResult<OtpSimRefundData>>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<OtpSimBaseResult<string>> ReportMessage(OtpSimPhoneRequestResult phoneRequestResult, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "/sessions/report")
                .WithParam("token", ApiKey)
                .WithParam("session", phoneRequestResult.Session))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<OtpSimBaseResult<string>>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<OtpSimBaseResult<OtpSimBalanceData>> UserBalance(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "/users/balance")
                .WithParam("token", ApiKey))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<OtpSimBaseResult<OtpSimBalanceData>>();
    }
}