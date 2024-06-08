using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Proxy;

namespace TqkLibrary.Net.Proxy.Wrapper.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public class AzProxyApiWrapper : IProxyApiWrapper
    {
        readonly AzProxyApi azProxyApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="azProxyApi"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AzProxyApiWrapper(AzProxyApi azProxyApi)
        {
            this.azProxyApi = azProxyApi ?? throw new ArgumentNullException(nameof(azProxyApi));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public AzProxyApiWrapper(string apiKey)
        {
            this.azProxyApi = new AzProxyApi(apiKey);
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
        public async Task<IProxyApiResponseWrapper> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            var proxy = await azProxyApi.GetNewProxy(Location, Provider, cancellationToken).ConfigureAwait(false);
            var proxy2 = await azProxyApi.GetNewProxy(Location, Provider, cancellationToken).ConfigureAwait(false);
            DateTime nextTime = DateTime.Now.Add(proxy2.NextTime.HasValue ? proxy2.NextTime.Value : TimeSpan.FromSeconds(10));
            return new ProxyApiResponseWrapper()
            {
                Proxy = proxy.Data?.Proxy,
                IsSuccess = proxy.IsSuccess,
                NextTime = nextTime,
                ExpiredTime = nextTime.AddMinutes(20),
                Message = proxy?.Message,
                ProxyType = ProxyType.Http,
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"AzProxy ({azProxyApi.ApiKey})";
        }
    }
}
