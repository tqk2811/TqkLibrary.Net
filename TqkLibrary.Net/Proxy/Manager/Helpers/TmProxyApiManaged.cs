using System;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Proxy.ProxysApi;

namespace TqkLibrary.Net.Proxy.Manager.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class TmProxyApiManaged : IProxyApi
    {
        readonly TmProxyApi tmProxyApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmProxyApi"></param>
        public TmProxyApiManaged(TmProxyApi tmProxyApi)
        {
            this.tmProxyApi = tmProxyApi ?? throw new ArgumentNullException(nameof(tmProxyApi));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public TmProxyApiManaged(string apiKey)
        {
            this.tmProxyApi = new TmProxyApi(apiKey);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Location { get; set; } = 0;
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
            var result = await tmProxyApi.GetNewProxy(Location).ConfigureAwait(false);
            return new ProxyApiResponse()
            {
                IsSuccess = result.code == 0,
                Proxy = result?.data.https ?? string.Empty,
                NextTime = DateTime.Now.AddSeconds(result?.data.next_request ?? 5),
                ExpiredTime = result?.data.expired_at ?? DateTime.Now
            };
        }
    }
}
