using System.Threading.Tasks;
using System.Web;

namespace TqkLibrary.Net.PhoneNumberApi.RentCodeCo
{
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
      NetworkProvider networkProvider = NetworkProvider.None,
      ServiceProviderId serviceProviderId = ServiceProviderId.Facebook)
    {
      if (serviceProviderId == ServiceProviderId.None) throw new RentCodeException("serviceProviderId is required");

      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["apiKey"] = ApiKey;
      parameters["ServiceProviderId"] = ((int)serviceProviderId).ToString();
      if (networkProvider != NetworkProvider.None) parameters["NetworkProvider"] = ((int)networkProvider).ToString();
      if (MaximumSms != null) parameters["MaximumSms"] = MaximumSms.Value.ToString();
      if (AllowVoiceSms != null) parameters["AllowVoiceSms"] = AllowVoiceSms.Value.ToString();

      return RequestGet<RentCodeResult>(EndPoint + "order/request?" + parameters.ToString());
    }

    public Task<RentCodeResult> RequestHolding(
      int Duration = 300,
      int Unit = 1,
      NetworkProvider networkProvider = NetworkProvider.None)
    {
      var parameters = HttpUtility.ParseQueryString(string.Empty);
      parameters["apiKey"] = ApiKey;
      parameters["Duration"] = Duration.ToString();
      parameters["Unit"] = Unit.ToString();
      if (networkProvider != NetworkProvider.None) parameters["NetworkProvider"] = ((int)networkProvider).ToString();

      return RequestGet<RentCodeResult>(EndPoint + $"order/request-holding?apiKey={ApiKey}&Duration={Duration}&Unit=1&NetworkProvider={(int)networkProvider}");
    }

    public Task<RentCodeCheckOrderResults> Check(RentCodeResult rentCodeResult)
      => RequestGet<RentCodeCheckOrderResults>(EndPoint + $"order/{rentCodeResult.Id}/check?apiKey={ApiKey}");
  }
}