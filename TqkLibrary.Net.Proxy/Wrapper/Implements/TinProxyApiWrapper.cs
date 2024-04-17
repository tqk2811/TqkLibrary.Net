using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxy.Wrapper.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public class TinProxyApiWrapper : IProxyApiWrapper
    {
        readonly TinProxyApi tinProxyApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public TinProxyApiWrapper(string apiKey)
        {
            this.tinProxyApi = new TinProxyApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tinProxyApi"></param>
        public TinProxyApiWrapper(TinProxyApi tinProxyApi)
        {
            this.tinProxyApi = tinProxyApi;
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsAllowGetNewOnUsing => true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IProxyApiResponseWrapper> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            var res = await tinProxyApi.GetNewProxy(cancellationToken).ConfigureAwait(false);

            return new ProxyApiResponseWrapper()
            {
                IsSuccess = res.Code == 1,
                Proxy = res.Data.HttpIpv4,
                NextTime = DateTime.Now.AddSeconds(res.Data.NextRequest),
                ExpiredTime = DateTime.Now.AddSeconds(res.Data.Timeout),
                Message = res.Message
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return tinProxyApi.ToString();
        }
    }
}
