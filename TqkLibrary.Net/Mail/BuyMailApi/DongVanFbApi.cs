using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Mail.BuyMailApi
{
    /// <summary>
    /// 
    /// </summary>
    public class DongVanFbApi : BaseApi
    {
        const string EndPoint = "https://dongvanfb.com/api/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="cancellationToken"></param>
        public DongVanFbApi(string apiKey, CancellationToken cancellationToken = default) : base(apiKey, cancellationToken)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<DongVanFbInfo> Info() => RequestGet<DongVanFbInfo>($"{EndPoint}info.php?apiKey={ApiKey}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Task<DongVanFbBuyAccount> BuyAccount(DongVanFbProduct product, int amount)
          => RequestGet<DongVanFbBuyAccount>($"{EndPoint}buyaccount.php?apiKey={ApiKey}&type={product.type}&amount={amount}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<DongVanFbOrderCode> OrderCode(DongVanFbAccount account)
          => RequestGet<DongVanFbOrderCode>($"{EndPoint}ordercode.php?apiKey={ApiKey}&type={account.type}&user={account.user}&pass={account.pass}");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Task<DongVanFbGetCode> GetCode(DongVanFbOrder order)
          => RequestGet<DongVanFbGetCode>($"{EndPoint}getcode.php?apiKey={ApiKey}&id={order.id}");
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
