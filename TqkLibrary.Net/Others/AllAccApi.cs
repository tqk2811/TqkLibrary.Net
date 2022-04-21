using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Others
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class AllAccApiResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }
    }

    public class AllAccApiBalanceResponse : AllAccApiResponse
    {
        [JsonProperty("balance")]
        public long Balance { get; set; }
    }
    public class AllAccApiBuyResponse : AllAccApiResponse
    {
        [JsonProperty("order_id")]
        public long OrderId { get; set; }
    }
    public class AllAccOrderItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("total_price")]
        public long TotalPrice { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }


        public static implicit operator AllAccOrderItem(int id)
        {
            return new AllAccOrderItem { Id = id };
        }

    }
    public class AllAccOrderResponse<T> : AllAccData<T>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("total_price")]
        public long TotalPrice { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
    public class AllAccData<T>
    {
        [JsonProperty("data")]
        public List<T> Categories { get; set; }
    }
    public class AllAccCategory
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("list_product")]
        public List<AllAccProduct> Products { get; set; }

        public static implicit operator AllAccCategory(int id)
        {
            return new AllAccCategory { Id = id };
        }
    }
    public class AllAccProduct
    {
        [JsonProperty("id_product")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        public static implicit operator AllAccProduct(int id)
        {
            return new AllAccProduct { Id = id };
        }
    }
    public interface IAllAccOrderItem
    {

    }
    public class AllAccOrderItemFacebook : IAllAccOrderItem
    {
        [JsonProperty("uid")]
        public int Uid { get; set; }

        [JsonProperty("full_info")]
        public string FullInfo { get; set; }

        [JsonProperty("link_backup")]
        public string LinkBackup { get; set; }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member



    /// <summary>
    /// 
    /// </summary>
    public class AllAccApi : BaseApi
    {
        const string EndPoint = "http://allacc.vn/api/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public AllAccApi(string apiKey) : base(apiKey)
        {

        }

        class ApiPost
        {
            public string api_key { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AllAccApiBalanceResponse> Balance()
        {
            ApiPost balancePost = new ApiPost();
            balancePost.api_key = ApiKey;
            using StringContent stringContent = new StringContent(JsonConvert.SerializeObject(balancePost), Encoding.UTF8, "application/json");
            return await RequestPostAsync<AllAccApiBalanceResponse>(EndPoint + "v1/balance", httpContent: stringContent).ConfigureAwait(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<AllAccData<AllAccCategory>> Categories()
        {
            return RequestGetAsync<AllAccData<AllAccCategory>>(EndPoint + "v1/categories");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Task<AllAccData<AllAccProduct>> Product(AllAccCategory category)
        {
            return RequestGetAsync<AllAccData<AllAccProduct>>(EndPoint + $"v1/category/{category.Id}");
        }

        class BuyPost : ApiPost
        {
            public int id_product { get; set; }
            public int quantity { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="buyCount"></param>
        /// <returns></returns>
        public async Task<AllAccApiBuyResponse> Buy(AllAccProduct product, int buyCount = 1)
        {
            BuyPost buyPost = new BuyPost();
            buyPost.api_key = ApiKey;
            buyPost.id_product = product.Id;
            buyPost.quantity = buyCount;
            using StringContent stringContent = new StringContent(JsonConvert.SerializeObject(buyPost), Encoding.UTF8, "application/json");
            return await RequestPostAsync<AllAccApiBuyResponse>(EndPoint + "v1/buy", httpContent: stringContent).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AllAccData<AllAccOrderItem>> Orders()
        {
            ApiPost ordersPost = new ApiPost();
            ordersPost.api_key = ApiKey;
            using StringContent stringContent = new StringContent(JsonConvert.SerializeObject(ordersPost), Encoding.UTF8, "application/json");
            return await RequestPostAsync<AllAccData<AllAccOrderItem>>(EndPoint + "v1/orders", httpContent: stringContent).ConfigureAwait(false);
        }

        class OrderPost : ApiPost
        {
            public long order_id { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<AllAccOrderResponse<T>> Order<T>(AllAccOrderItem order) where T : IAllAccOrderItem
        {
            OrderPost orderPost = new OrderPost();
            orderPost.api_key = ApiKey;
            orderPost.order_id = order.Id;
            using StringContent stringContent = new StringContent(JsonConvert.SerializeObject(orderPost), Encoding.UTF8, "application/json");
            return await RequestPostAsync<AllAccOrderResponse<T>>(EndPoint + "v1/order", httpContent: stringContent).ConfigureAwait(false);
        }
    }


}
