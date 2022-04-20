using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TqkLibrary.Net.Phone.PhoneApi
{
  public enum ChoThueSimResponseCode
  {
    Success = 0,
    Error = 1,
  }

  public enum ChoThueSimResponseCodeGetPhoneNumber
  {
    Success = 0,
    WalletNotEnough = 1,
    AppNotExist = 2,
    PhoneNumberIsTemporarilyRunningOut = 3
  }

  public enum ChoThueSimResponseCodeMessage
  {
    Success = 0,
    Waitting = 1,
    Timeout = 2,
    InputIsCorrect = 3
  }

  public enum ChoThueSimResponseCodeCancelMessage
  {
    Success = 0,
    IdNotFound = 1,
    WasCanceled = 2
  }
  public class ChoThueSimRefundInfo
  {
    public double Balance { get; set; }
    public double Refund { get; set; }
  }
  public class ChoThueSimPhoneNumberResult
  {
    public int Id { get; set; }
    public string Number { get; set; }
    public string App { get; set; }
    public double Cost { get; set; }
    public double Balance { get; set; }
  }
  public class ChoThueSimMessageResult
  {
    public string SMS { get; set; }
    public string Code { get; set; }
    public double? Cost { get; set; }
    public bool? IsCall { get; set; }
    public string CallFile { get; set; }
    public string CallText { get; set; }
  }
  public enum ChoThueSimCarrier
  {
    None, Viettel, Mobi, Vina, VNMB
  }
  public class ChoThueSimBaseResult<T1, T2>
  {
    public T1 ResponseCode { get; set; }
    public string Msg { get; set; }
    public T2 Result { get; set; }
  }
  public class ChoThueSimAppInfo
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public double Cost { get; set; }
  }
  public class ChoThueSimAccountInfo
  {
    public string Name { get; set; }
    public string Phone { get; set; }
    public double Balance { get; set; }
  }
  /// <summary>
  /// https://chothuesimcode.com/account/api
  /// </summary>
  public sealed class ChoThueSimCodeApi : BaseApi
  {
    private const string EndPoint = "https://chothuesimcode.com/api?";

    public ChoThueSimCodeApi(string ApiKey, CancellationToken cancellationToken = default) : base(ApiKey,cancellationToken)
    {
    }

    public Task<ChoThueSimBaseResult<ChoThueSimResponseCode, ChoThueSimAccountInfo>> GetAccountInfo()
      => RequestGet<ChoThueSimBaseResult<ChoThueSimResponseCode, ChoThueSimAccountInfo>>(string.Format(EndPoint + "act=account&apik={0}", ApiKey));

    public Task<ChoThueSimBaseResult<ChoThueSimResponseCode, List<ChoThueSimAppInfo>>> GetAppRunning()
      => RequestGet<ChoThueSimBaseResult<ChoThueSimResponseCode, List<ChoThueSimAppInfo>>>(string.Format(EndPoint + "act=app&apik={0}", ApiKey));

    public Task<ChoThueSimBaseResult<ChoThueSimResponseCodeGetPhoneNumber, ChoThueSimPhoneNumberResult>> GetPhoneNumber(ChoThueSimAppInfo app, ChoThueSimCarrier carrier = ChoThueSimCarrier.None)
    {
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["act"] = "number";
      parameters["apik"] = ApiKey;
      parameters["appId"] = app.Id.ToString();
      if (carrier != ChoThueSimCarrier.None) parameters["carrier"] = carrier.ToString();
      return RequestGet<ChoThueSimBaseResult<ChoThueSimResponseCodeGetPhoneNumber, ChoThueSimPhoneNumberResult>>(EndPoint + parameters.ToString());
    }

    public Task<ChoThueSimBaseResult<ChoThueSimResponseCodeGetPhoneNumber, ChoThueSimPhoneNumberResult>> GetPhoneNumber(int appId, string number)
    {
      if (string.IsNullOrEmpty(number)) throw new ArgumentNullException(nameof(number));
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["act"] = "number";
      parameters["apik"] = ApiKey;
      parameters["appId"] = appId.ToString();
      parameters["number"] = number;
      return RequestGet<ChoThueSimBaseResult<ChoThueSimResponseCodeGetPhoneNumber, ChoThueSimPhoneNumberResult>>(EndPoint + parameters.ToString());
    }

    public Task<ChoThueSimBaseResult<ChoThueSimResponseCodeMessage, ChoThueSimMessageResult>> GetMessage(ChoThueSimPhoneNumberResult phoneNumberResult)
    {
      if (null == phoneNumberResult) throw new ArgumentNullException(nameof(phoneNumberResult));
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["act"] = "code";
      parameters["apik"] = ApiKey;
      parameters["id"] = phoneNumberResult.Id.ToString();
      return RequestGet<ChoThueSimBaseResult<ChoThueSimResponseCodeMessage, ChoThueSimMessageResult>>(EndPoint + parameters.ToString());
    }

    public Task<ChoThueSimBaseResult<ChoThueSimResponseCodeCancelMessage, ChoThueSimRefundInfo>> CancelGetMessage(ChoThueSimPhoneNumberResult phoneNumberResult)
    {
      if (null == phoneNumberResult) throw new ArgumentNullException(nameof(phoneNumberResult));
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["act"] = "expired";
      parameters["apik"] = ApiKey;
      parameters["id"] = phoneNumberResult.Id.ToString();
      return RequestGet<ChoThueSimBaseResult<ChoThueSimResponseCodeCancelMessage, ChoThueSimRefundInfo>>(EndPoint + parameters.ToString());
    }
  }
}