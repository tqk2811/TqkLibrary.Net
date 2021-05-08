using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TqkLibrary.Net.PhoneNumberApi.OtpSimCom
{
  public sealed class OtpSimApi : BaseApi
  {
    private const string EndPoint = "http://otpsim.com/api";

    public OtpSimApi(string ApiKey) : base(ApiKey)
    {
    }

    public Task<BaseResult<List<DataNetwork>>> GetNetworks()
      => RequestGet<BaseResult<List<DataNetwork>>>(string.Format(EndPoint + "/networks?token={0}", ApiKey));

    public Task<BaseResult<List<DataService>>> GetServices()
      => RequestGet<BaseResult<List<DataService>>>(string.Format(EndPoint + "/service/request?token={0}", ApiKey));

    public Task<BaseResult<PhoneRequestResult>> PhonesRequest(
      DataService dataService,
      IEnumerable<DataNetwork> dataNetworks = null,
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

      return RequestGet<BaseResult<PhoneRequestResult>>(EndPoint + "/phones/request?" + parameters.ToString());
    }

    public Task<BaseResult<PhoneRequestResult>> PhonesRequest(DataService dataService, string numberBuyBack)
    {
      if (null == dataService) throw new ArgumentNullException(nameof(dataService));
      if (string.IsNullOrEmpty(numberBuyBack)) throw new ArgumentNullException(nameof(numberBuyBack));

      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["token"] = ApiKey;
      parameters["service"] = dataService.Id.ToString();
      parameters["numberBuyBack"] = numberBuyBack;

      return RequestGet<BaseResult<PhoneRequestResult>>(EndPoint + "/phones/request?" + parameters.ToString());
    }

    public Task<BaseResult<PhoneData>> GetPhoneMessage(PhoneRequestResult phoneRequestResult)
      => RequestGet<BaseResult<PhoneData>>($"{EndPoint}/sessions/{phoneRequestResult.Session}?token={ApiKey}");

    public Task<BaseResult<RefundData>> CancelGetPhoneMessage(PhoneRequestResult phoneRequestResult)
      => RequestGet<BaseResult<RefundData>>($"{EndPoint}/sessions/cancel?session={phoneRequestResult.Session}&token={ApiKey}");

    public Task<BaseResult<string>> ReportMessage(PhoneRequestResult phoneRequestResult)
      => RequestGet<BaseResult<string>>($"{EndPoint}/sessions/report?session={phoneRequestResult.Session}&token={ApiKey}");

    public Task<BaseResult<BalanceData>> UserBalance()
       => RequestGet<BaseResult<BalanceData>>($"{EndPoint}users/balance?token={ApiKey}");
  }
}