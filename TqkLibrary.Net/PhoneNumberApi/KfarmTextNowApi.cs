using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.PhoneNumberApi
{
  public class KfarmTextNowApi : BaseApi
  {
    const string EndPoint = "http://kfarm.vn/api/TextNow/";
    readonly Dictionary<string, string> headers = new Dictionary<string, string>();
    public KfarmTextNowApi(string token, CancellationToken cancellationToken = default) : base(token, cancellationToken)
    {
      headers.Add("Token", token);
    }
    public Task<KfarmTextNowResponse<KfarmTextNowPhone>> GetAccTextNow()
    {
      return RequestGet<KfarmTextNowResponse<KfarmTextNowPhone>>($"{EndPoint}GetAccTextNow", headers);
    }

    public Task<KfarmTextNowResponse<KfarmTextNowOrder>> GetOrderTextNow(KfarmTextNowPhone phone)
    {
      return RequestPost<KfarmTextNowResponse<KfarmTextNowOrder>>(
        $"{EndPoint}GetOrderTextNow", 
        headers,
        new StringContent(JsonConvert.SerializeObject(phone),Encoding.UTF8,"application/json"));
    }



    public Task<KfarmTextNowResponse<KfarmTextNowCode>> GetCode(KfarmTextNowOrder orderId)
    {
      return RequestPost<KfarmTextNowResponse<KfarmTextNowCode>>(
        $"{EndPoint}GetCode",
        headers,
        new StringContent(JsonConvert.SerializeObject(orderId), Encoding.UTF8, "application/json"));
    }
  }


  public class KfarmTextNowResponse<T>
  {
    public int status_code { get; set; }
    public string message { get; set; }
    public T data { get; set; }
  }
  public class KfarmTextNowPhone
  {
    public string phone { get; set; }
  }
  public class KfarmTextNowOrder
  {
    public string order_id { get; set; }
  }
  public class KfarmTextNowCode
  {
    public string code { get; set; }
  }
}
