using System;
using System.Threading.Tasks;
using System.Web;

namespace TqkLibrary.Net.PhoneNumberApi.ChoThueSimCodeCom
{
  /// <summary>
  /// https://chothuesimcode.com/account/api
  /// </summary>
  public sealed class ChoThueSimCodeApi : BaseApi
  {
    private const string EndPoint = "https://chothuesimcode.com/api?";

    public ChoThueSimCodeApi(string ApiKey) : base(ApiKey)
    {
    }

    public Task<BaseResult<ResponseCode, AccountInfo>> GetAccountInfo()
      => RequestGet<BaseResult<ResponseCode, AccountInfo>>(string.Format(EndPoint + "act=account&apik={0}", ApiKey));

    public Task<BaseResult<ResponseCode, AppInfo>> GetAppRunning()
      => RequestGet<BaseResult<ResponseCode, AppInfo>>(string.Format(EndPoint + "act=app&apik={0}", ApiKey));

    public Task<BaseResult<ResponseCodeGetPhoneNumber, PhoneNumberResult>> GetPhoneNumber(int appId, Carrier carrier = Carrier.None)
    {
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["act"] = "number";
      parameters["apik"] = ApiKey;
      parameters["appId"] = appId.ToString();
      if (carrier != Carrier.None) parameters["carrier"] = carrier.ToString();
      return RequestGet<BaseResult<ResponseCodeGetPhoneNumber, PhoneNumberResult>>(EndPoint + parameters.ToString());
    }

    public Task<BaseResult<ResponseCodeGetPhoneNumber, PhoneNumberResult>> GetPhoneNumber(int appId, string number)
    {
      if (string.IsNullOrEmpty(number)) throw new ArgumentNullException(nameof(number));
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["act"] = "number";
      parameters["apik"] = ApiKey;
      parameters["appId"] = appId.ToString();
      parameters["number"] = number;
      return RequestGet<BaseResult<ResponseCodeGetPhoneNumber, PhoneNumberResult>>(EndPoint + parameters.ToString());
    }

    public Task<BaseResult<ResponseCodeMessage, MessageResult>> GetMessage(PhoneNumberResult phoneNumberResult)
    {
      if (null == phoneNumberResult) throw new ArgumentNullException(nameof(phoneNumberResult));
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["act"] = "code";
      parameters["apik"] = ApiKey;
      parameters["id"] = phoneNumberResult.Id.ToString();
      return RequestGet<BaseResult<ResponseCodeMessage, MessageResult>>(EndPoint + parameters.ToString());
    }

    public Task<BaseResult<ResponseCodeCancelMessage, RefundInfo>> CancelGetMessage(PhoneNumberResult phoneNumberResult)
    {
      if (null == phoneNumberResult) throw new ArgumentNullException(nameof(phoneNumberResult));
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["act"] = "expired";
      parameters["apik"] = ApiKey;
      parameters["id"] = phoneNumberResult.Id.ToString();
      return RequestGet<BaseResult<ResponseCodeCancelMessage, RefundInfo>>(EndPoint + parameters.ToString());
    }
  }
}