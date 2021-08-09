using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net
{
  public class SmmFansFasterViewServiceResult
  {
    public int service { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string category { get; set; }
    public double rate { get; set; }
    public int min { get; set; }
    public int max { get; set; }
    public bool dripfeed { get; set; }
    public bool refill { get; set; }

    public override string ToString()
    {
      return $"name: {name}, type: {type}, category: {category}";
    }
  }
  public class SmmFansFasterOrderResult
  {
    public int order { get; set; }
    public string error { get; set; }
  }

  public class SmmFansFasterApi : BaseApi
  {
    const string EndPoint = "https://smmfansfaster.com/api/v2";
    public SmmFansFasterApi(string ApiKey, CancellationToken cancellationToken = default) : base(ApiKey, cancellationToken)
    {

    }

    public Task<List<SmmFansFasterViewServiceResult>> ServiceList()
    {
      var formPost = new FormUrlEncodedContent(new[]
      {
        new KeyValuePair<string, string>("key", ApiKey),
        new KeyValuePair<string, string>("action", "services")
      });
      return RequestPost<List<SmmFansFasterViewServiceResult>>(EndPoint, null, formPost);
    }

    public Task<SmmFansFasterOrderResult> AddOrder(SmmFansFasterViewServiceResult smmFansFasterViewServiceResult,string link,int quantity)
    {
      var formPost = new FormUrlEncodedContent(new[]
      {
        new KeyValuePair<string, string>("key", ApiKey),
        new KeyValuePair<string, string>("action", "add"),
        new KeyValuePair<string, string>("service", smmFansFasterViewServiceResult.service.ToString()),
        new KeyValuePair<string, string>("link", link),
        new KeyValuePair<string, string>("quantity", quantity.ToString())
      });
      return RequestPost<SmmFansFasterOrderResult>(EndPoint, null, formPost);
    }
  }
}
