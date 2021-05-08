using System;
using System.Threading.Tasks;

namespace TqkLibrary.Net.PhoneNumberApi.SimThueCom
{
  /// <summary>
  /// https://simthue.com/vi/api/index
  /// </summary>
  public sealed class SimThueApi : BaseApi
  {
    private const string EndPoint = "http://api.simthue.com";

    public SimThueApi(string ApiKey) : base(ApiKey)
    {
    }

    public Task<BalanceResult> GetBalance()
      => RequestGet<BalanceResult>(string.Format(EndPoint + "/balance?key={0}", ApiKey));

    public Task<ServicesResult> GetAvailableServices()
      => RequestGet<ServicesResult>(string.Format(EndPoint + "/service?key={0}", ApiKey));

    public Task<RequestResult> CreateRequest(ServiceResult serviceResult)
    {
      if (null == serviceResult) throw new ArgumentNullException(nameof(serviceResult));
      return RequestGet<RequestResult>(string.Format(EndPoint + "/create?key={0}&service_id={1}", ApiKey, serviceResult.Id));
    }

    public Task<CheckResult> CheckRequest(RequestResult createResult)
    {
      if (null == createResult) throw new ArgumentNullException(nameof(createResult));
      return RequestGet<CheckResult>(string.Format(EndPoint + "/check?key={0}&id={1}", ApiKey, createResult.Id));
    }

    public Task<RequestResult> CancelRequest(RequestResult createResult)
    {
      if (null == createResult) throw new ArgumentNullException(nameof(createResult));
      return RequestGet<RequestResult>(string.Format(EndPoint + "/cancel?key={0}&id={1}", ApiKey, createResult.Id));
    }

    public Task<RequestResult> FinishRequest(RequestResult createResult)
    {
      if (null == createResult) throw new ArgumentNullException(nameof(createResult));
      return RequestGet<RequestResult>(string.Format(EndPoint + "/cancel?key={0}&id={1}", ApiKey, createResult.Id));
    }
  }
}