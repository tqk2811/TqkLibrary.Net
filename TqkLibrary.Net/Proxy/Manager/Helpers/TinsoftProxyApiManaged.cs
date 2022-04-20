using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Proxy.ProxysApi;
namespace TqkLibrary.Net.Proxy.Manager.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class TinsoftProxyApiManaged : IProxyApi
    {
        readonly TinsoftProxyApi tinsoftProxyApi;

        /// <summary>
        /// 
        /// </summary>
        public int Location { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public TinsoftProxyApiManaged(TinsoftProxyApi tinsoftProxyApi)
        {
            this.tinsoftProxyApi = tinsoftProxyApi ?? throw new ArgumentNullException(nameof(tinsoftProxyApi));
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
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IProxyApiResponse> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            var result = await tinsoftProxyApi.ChangeProxy(Location).ConfigureAwait(false);
            return new ProxyApiResponse()
            {
                IsSuccess = result.Success,
                Proxy = result.Proxy,
                NextTime = DateTime.Now.AddSeconds(result.NextChange),
                ExpiredTime = DateTime.Now.AddSeconds(result.Timeout)
            };
        }
    }
}
