using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TqkLibrary.Net.PhoneNumberApi
{
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
  public sealed class OtpSimApi : BaseApi
  {
    private const string EndPoint = "http://otpsim.com/api";

    public OtpSimApi(string ApiKey, CancellationToken cancellationToken = default) : base(ApiKey,cancellationToken)
    {
    }

    public Task<OtpSimBaseResult<List<OtpSimDataNetwork>>> GetNetworks()
      => RequestGet<OtpSimBaseResult<List<OtpSimDataNetwork>>>(string.Format(EndPoint + "/networks?token={0}", ApiKey));

    public Task<OtpSimBaseResult<List<OtpSimDataService>>> GetServices()
      => RequestGet<OtpSimBaseResult<List<OtpSimDataService>>>(string.Format(EndPoint + "/service/request?token={0}", ApiKey));

    public Task<OtpSimBaseResult<OtpSimPhoneRequestResult>> PhonesRequest(
      OtpSimDataService dataService,
      IEnumerable<OtpSimDataNetwork> dataNetworks = null,
      IEnumerable<string> prefixs = null,
      IEnumerable<string> exceptPrefixs = null)
    {
      if (null == dataService) throw new ArgumentNullException(nameof(dataService));

      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["token"] = ApiKey;
      parameters["service"] = dataService.Id.ToString();
      if (null != dataNetworks) parameters["network"] = string.Join(",", dataNetworks.Select(x => x.Id));
      if (null != prefixs) parameters["prefix"] = string.Join(",", prefixs);
      if (null != exceptPrefixs) parameters["exceptPrefix"] = string.Join(",", exceptPrefixs);

      return RequestGet<OtpSimBaseResult<OtpSimPhoneRequestResult>>(EndPoint + "/phones/request?" + parameters.ToString());
    }

    public Task<OtpSimBaseResult<OtpSimPhoneRequestResult>> PhonesRequest(OtpSimDataService dataService, string numberBuyBack)
    {
      if (null == dataService) throw new ArgumentNullException(nameof(dataService));
      if (string.IsNullOrEmpty(numberBuyBack)) throw new ArgumentNullException(nameof(numberBuyBack));

      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["token"] = ApiKey;
      parameters["service"] = dataService.Id.ToString();
      parameters["numberBuyBack"] = numberBuyBack;

      return RequestGet<OtpSimBaseResult<OtpSimPhoneRequestResult>>(EndPoint + "/phones/request?" + parameters.ToString());
    }

    public Task<OtpSimBaseResult<OtpSimPhoneData>> GetPhoneMessage(OtpSimPhoneRequestResult phoneRequestResult)
      => RequestGet<OtpSimBaseResult<OtpSimPhoneData>>($"{EndPoint}/sessions/{phoneRequestResult.Session}?token={ApiKey}");

    public Task<OtpSimBaseResult<OtpSimRefundData>> CancelGetPhoneMessage(OtpSimPhoneRequestResult phoneRequestResult)
      => RequestGet<OtpSimBaseResult<OtpSimRefundData>>($"{EndPoint}/sessions/cancel?session={phoneRequestResult.Session}&token={ApiKey}");

    public Task<OtpSimBaseResult<string>> ReportMessage(OtpSimPhoneRequestResult phoneRequestResult)
      => RequestGet<OtpSimBaseResult<string>>($"{EndPoint}/sessions/report?session={phoneRequestResult.Session}&token={ApiKey}");

    public Task<OtpSimBaseResult<OtpSimBalanceData>> UserBalance()
       => RequestGet<OtpSimBaseResult<OtpSimBalanceData>>($"{EndPoint}users/balance?token={ApiKey}");
  }
}