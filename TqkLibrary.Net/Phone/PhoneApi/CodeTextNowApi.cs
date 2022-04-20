using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Phone.PhoneApi
{
  public class CodeTextNowApi : BaseApi
  {
    const string EndPoint = "http://codetextnow.com/api.php";
    public CodeTextNowApi(string apiKey, CancellationToken cancellationToken = default) : base(apiKey,cancellationToken)
    {

    }


    public Task<List<CodeTextNowService>> Services()
      => RequestGet<List<CodeTextNowService>>($"{EndPoint}?apikey={ApiKey}&action=services");

    public Task<CodeTextNowCreateRequest> CreateRequest(CodeTextNowService service, int count = 1)
      => RequestGet<CodeTextNowCreateRequest>($"{EndPoint}?apikey={ApiKey}&action=create-request&serviceId={service.serviceId}&count={count}");

    public Task<CodeTextNowDataRequest> DataRequest(CodeTextNowRent rent)
     => RequestGet<CodeTextNowDataRequest>($"{EndPoint}?apikey={ApiKey}&action=data-request&requestId={rent.requestId}");

    //public void ReNewRequest()
    //{

    //}

  }

  public class CodeTextNowCreateRequest: CodeTextNowResponse<CodeTextNowResultResponse<List<CodeTextNowRent>>> { }
  public class CodeTextNowDataRequest : CodeTextNowResponse2<List<CodeTextNowData>> { }





  public class CodeTextNowService
  {
    public string name { get; set; }
    public int serviceId { get; set; }
    public int price { get; set; }
    public int timeout { get; set; }
  }



  public class CodeTextNowResponse<T>
  {
    public int status { get; set; }
    public T results { get; set; }
    public string message { get; set; }
  }
  

  public class CodeTextNowResultResponse<T>
  {
    public T data { get; set; }
  }

  public class CodeTextNowRent
  {
    public string name { get; set; }
    public string sdt { get; set; }
    public string otp { get; set; }
    public int status { get; set; }
    public long created_time { get; set; }
    public int requestId { get; set; }
  }






  public class CodeTextNowResponse2<T>
  {
    public string message { get; set; }
    public int status { get; set; }
    public T data { get; set; }
    public int? recordsFiltered { get; set; }
    public int? recordsTotal { get; set; }
  }
  public class CodeTextNowData
  {
    public int id { get; set; }
    public string name { get; set; }
    public int price { get; set; }
    public string sdt { get; set; }
    public string otp { get; set; }
    public string textSMS { get; set; }
    public long finishtime { get; set; }
    public int status { get; set; }
    public string created_time { get; set; }
  }
}
