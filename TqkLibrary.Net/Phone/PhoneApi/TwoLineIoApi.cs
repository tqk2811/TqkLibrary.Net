﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace TqkLibrary.Net.Phone.PhoneApi
{
    /// <summary>
    /// https://2ndline.io/documentapi
    /// </summary>
    public class TwoLineIoApi : BaseApi
    {
        static readonly string EndPoint = "https://2ndline.io/apiv1";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public TwoLineIoApi(string apiKey) : base(apiKey, NetSingleton.httpClient)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TwoLineIoResponseBalance> GetBalance(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "getbalance")
                .WithParam("apikey", ApiKey))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TwoLineIoResponseBalance>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<TwoLineIoService>> GetServices(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "availableservice"))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<List<TwoLineIoService>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="netWorkId"></param>
        /// <param name="allowVoiceSms"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TwoLineIoPurchaseOtpResponse> PurchaseOTP(
            TwoLineIoService service,
            TwoLineIoNetWorkId? netWorkId = null,
            bool? allowVoiceSms = null,
            CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "order")
                .WithParam("apikey", ApiKey)
                .WithParam("serviceId", service.ServiceId)
                .WithParamIfNotNull("networkId", netWorkId.HasValue ? (int?)netWorkId.Value : null)
                .WithParamIfNotNull("allowVoiceSms", allowVoiceSms))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TwoLineIoPurchaseOtpResponse>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseOtpResponse"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TwoLineIoData<TwoLineIoOrderData>> CheckOrder(
            TwoLineIoPurchaseOtpResponse purchaseOtpResponse,
            CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "checkorder")
                .WithParam("apikey", ApiKey)
                .WithParam("id", purchaseOtpResponse.Id))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TwoLineIoData<TwoLineIoOrderData>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseOtpResponse"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TwoLineIoPurchaseOtpResponse> ContinueOrder(
            TwoLineIoPurchaseOtpResponse purchaseOtpResponse,
            CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "continueorder")
                .WithParam("apikey", ApiKey)
                .WithParam("orderId", purchaseOtpResponse.Id))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TwoLineIoPurchaseOtpResponse>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<TwoLineIoTimeHolding>> TimeHoldings(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "availabletime")
                .WithParam("apikey", ApiKey))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<List<TwoLineIoTimeHolding>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseOtpResponse"></param>
        /// <param name="time"></param>
        /// <param name="holdUnit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TwoLineIoPurchaseOtpResponse> Holding(
            TwoLineIoPurchaseOtpResponse purchaseOtpResponse,
            int time,
            TwoLineIoHoldUnit holdUnit,
            CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "keepsim")
                .WithParam("apikey", ApiKey)
                .WithParam("orderId", purchaseOtpResponse.Id)
                .WithParam("time", time)
                .WithParam("unit", holdUnit))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TwoLineIoPurchaseOtpResponse>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TwoLineIoData<List<TwoLineIoPhoneKept>>> PhoneKepts(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "availablephone")
                .WithParam("apikey", ApiKey))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TwoLineIoData<List<TwoLineIoPhoneKept>>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="phoneKept"></param>
        /// <param name="allowVoiceSms"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TwoLineIoPurchaseOtpResponse> PurchageOrderFromPhoneHold(
            TwoLineIoService service,
            TwoLineIoPhoneKept phoneKept,
            bool? allowVoiceSms = null,
            CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "orderbyphone")
                .WithParam("apikey", ApiKey)
                .WithParam("serviceId", service.ServiceId)
                .WithParam("phone", phoneKept.Phone)
                .WithParamIfNotNull("allowVoiceSms", allowVoiceSms))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TwoLineIoPurchaseOtpResponse>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<TwoLineIoCountry>> GetAvailableCountry(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "availablecountry"))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<List<TwoLineIoCountry>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete("Can't test")]
        public Task<TwoLineIoOperator> GetAvailableoperator(TwoLineIoCountry country, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "availableoperator").WithParam("countryId", country.CountryId))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<TwoLineIoOperator>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="codeOperator"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<TwoLineIoService>> GetServicesByContries(
            TwoLineIoCountry country,
            TwoLineIoOperator codeOperator = null,
            CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "availableservice")
                .WithParam("countryId", country.CountryId)
                .WithParamIfNotNull("operatorId", codeOperator?.OperatorId))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<List<TwoLineIoService>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="service"></param>
        /// <param name="operator"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task PurchaseOtpCountry(
            TwoLineIoCountry country,
            TwoLineIoService service,
            TwoLineIoOperator @operator = null,
            CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(EndPoint, "availableservice")
                .WithParam("apikey", ApiKey)
                .WithParam("countryId", country.CountryId)
                .WithParam("serviceId", service.ServiceId)
                .WithParamIfNotNull("operatorId", @operator?.OperatorId))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<List<TwoLineIoService>>();

    }



#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TwoLineIoOperator
    {
        [JsonProperty("operatorId")]
        public string OperatorId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class TwoLineIoCountry
    {
        [JsonProperty("countryId")]
        public int CountryId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class TwoLineIoPhoneKept
    {
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }
    public enum TwoLineIoHoldUnit
    {
        Date,
        Hours,
        Minute
    }
    public class TwoLineIoTimeHolding
    {
        [JsonProperty("time")]
        public string Time { get; set; }
        [JsonProperty("timeUnit")]
        public string TimeUnit { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
    public enum TwoLineIoNetWorkId
    {
        Viettel = 1,
        MobiFone = 2,
        VinaPhone = 3,
        Vietnamobile = 4,
        ITelecom = 5
    }
    public class TwoLineIoResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class TwoLineIoResponseBalance : TwoLineIoResponse
    {
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
    }

    public class TwoLineIoPurchaseOtpResponse : TwoLineIoResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
    public enum TwoLineIoStatusOrder
    {
        Cancel = -1,
        Wait = 0,
        Success = 1,
    }
    public class TwoLineIoData<T> : TwoLineIoResponse
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
    public class TwoLineIoOrderData
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("haveVoice")]
        public bool HaveVoice { get; set; }
        [JsonProperty("audioUrl")]
        public string AudioUrl { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("statusOrder")]
        public TwoLineIoStatusOrder StatusOrder { get; set; }
    }
    public class TwoLineIoService
    {
        [JsonProperty("serviceId")]
        public int ServiceId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("lockTime")]
        public int LockTime { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("priceCall")]
        public decimal PriceCall { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
