using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.RentMails
{
  public class DongVanFbApi : BaseApi
  {
    const string EndPoint = "https://dongvanfb.com/api/";
    public DongVanFbApi(string apiKey, CancellationToken cancellationToken = default) : base(apiKey,cancellationToken)
    {

    }

    public Task<DongVanFbInfo> Info() => RequestGet<DongVanFbInfo>($"{EndPoint}info.php?apiKey={ApiKey}");

    public Task<DongVanFbBuyAccount> BuyAccount(DongVanFbProduct product, int amount)
      => RequestGet<DongVanFbBuyAccount>($"{EndPoint}buyaccount.php?apiKey={ApiKey}&type={product.type}&amount={amount}");

    public Task<DongVanFbOrderCode> OrderCode(DongVanFbAccount account)
      => RequestGet<DongVanFbOrderCode>($"{EndPoint}ordercode.php?apiKey={ApiKey}&type={account.type}&user={account.user}&pass={account.pass}");

    public Task<DongVanFbGetCode> GetCode(DongVanFbOrder order)
      => RequestGet<DongVanFbGetCode>($"{EndPoint}getcode.php?apiKey={ApiKey}&id={order.id}");
  }

  public class DongVanFbInfo : DongVanFbResponse<List<DongVanFbProduct>> { }
  public class DongVanFbBuyAccount : DongVanFbResponse<List<DongVanFbAccount>> { }
  public class DongVanFbOrderCode : DongVanFbResponse<DongVanFbOrder> { }
  public class DongVanFbGetCode : DongVanFbResponse<DongVanFbCode> { }



  public class DongVanFbResponse<T>
  {
    public int success { get; set; }
    public string message { get; set; }
    public int balance { get; set; }
    public T product { get; set; }
  }

  public class DongVanFbProduct
  {
    public string name { get; set; }
    public int price { get; set; }
    public int amount { get; set; }
    public int type { get; set; }
  }

  public class DongVanFbAccount//no api example, wait test
  {
    public int type { get; set; }
    public string user { get; set; }
    public string pass { get; set; }
  }

  public class DongVanFbOrder//no api example, wait test
  {
    public int id { get; set; }
  }

  public class DongVanFbCode//no api example, wait test
  {
    public string code { get; set; }
  }
}
