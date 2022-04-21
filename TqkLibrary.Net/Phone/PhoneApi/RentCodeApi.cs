using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TqkLibrary.Net.Phone.PhoneApi
{
    public sealed class RentCodeException : Exception
    {
        internal RentCodeException(string Message)
        {
        }
    }
    public enum RentCodeServiceProviderId : int
    {
        None = 0,
        Facebook = 3,
    }
    public sealed class RentCodeSmsMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
    public sealed class RentCodeResult
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("success")]
        public bool? Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return $"Id: {Id},Success: {Success},Message: {Message}";
        }
    }
    public enum RentCodeNetworkProvider : int
    {
        None = 0,
        Viettel = 1,
        VinaPhone = 2,
        MobilePhone = 3,
        VietnamMobile = 4,
        Cambodia = 5,
        ITelecom = 6
    }
    public sealed class RentCodeCheckOrderResults
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("messages")]
        public List<RentCodeSmsMessage> Messages { get; set; }

        public override string ToString()
        {
            return $"Success: {Success}, PhoneNumberApi: {PhoneNumber}, Message: {Message}";
        }
    }
    public sealed class RentCodeApi : BaseApi
    {
        private const string EndPoint = "https://api.rentcode.net/api/v2/";

        public RentCodeApi(string ApiKey) : base(ApiKey)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="MaximumSms"></param>
        /// <param name="AllowVoiceSms"></param>
        /// <param name="networkProvider"></param>
        /// <param name="serviceProviderId"></param>
        /// <exception cref="RentCodeException"></exception>
        /// <returns></returns>
        public Task<RentCodeResult> Request(
          int? MaximumSms = null,
          bool? AllowVoiceSms = null,
          RentCodeNetworkProvider networkProvider = RentCodeNetworkProvider.None,
          RentCodeServiceProviderId serviceProviderId = RentCodeServiceProviderId.Facebook)
        {
            if (serviceProviderId == RentCodeServiceProviderId.None) throw new RentCodeException("serviceProviderId is required");

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["apiKey"] = ApiKey;
            parameters["ServiceProviderId"] = ((int)serviceProviderId).ToString();
            if (networkProvider != RentCodeNetworkProvider.None) parameters["NetworkProvider"] = ((int)networkProvider).ToString();
            if (MaximumSms != null) parameters["MaximumSms"] = MaximumSms.Value.ToString();
            if (AllowVoiceSms != null) parameters["AllowVoiceSms"] = AllowVoiceSms.Value.ToString();

            return RequestGetAsync<RentCodeResult>(EndPoint + "order/request?" + parameters.ToString());
        }

        public Task<RentCodeResult> RequestHolding(
          int Duration = 300,
          int Unit = 1,
          RentCodeNetworkProvider networkProvider = RentCodeNetworkProvider.None)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["apiKey"] = ApiKey;
            parameters["Duration"] = Duration.ToString();
            parameters["Unit"] = Unit.ToString();
            if (networkProvider != RentCodeNetworkProvider.None) parameters["NetworkProvider"] = ((int)networkProvider).ToString();

            return RequestGetAsync<RentCodeResult>(EndPoint + $"order/request-holding?apiKey={ApiKey}&Duration={Duration}&Unit=1&NetworkProvider={(int)networkProvider}");
        }

        public Task<RentCodeCheckOrderResults> Check(RentCodeResult rentCodeResult)
          => RequestGetAsync<RentCodeCheckOrderResults>(EndPoint + $"order/{rentCodeResult.Id}/check?apiKey={ApiKey}");
    }
}