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
    public class AzProxyApiManaged : IProxyApi
    {
        readonly AzProxyApi azProxyApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="azProxyApi"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AzProxyApiManaged(AzProxyApi azProxyApi)
        {
            this.azProxyApi = azProxyApi ?? throw new ArgumentNullException(nameof(azProxyApi));
        }
        /// <summary>
        /// 
        /// </summary>
        public AzProxyLocation? Location { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public AzProxyProvider? Provider { get; set; }


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
            var proxy = await azProxyApi.GetNewProxy(Location, Provider, cancellationToken);
            return new ProxyApiResponse()
            {
                Proxy = proxy.Proxy,
                IsSuccess = proxy.Status.Equals("success"),
                NextTime = proxy.NextTime,
                ExpiredTime = DateTime.Now.AddDays(1)
            };
        }
    }
}
