using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Proxys;
namespace TqkLibrary.Net.Proxys.Wrapper.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class TinsoftProxyApiWrapper : IProxyApiWrapper
    {
        readonly TinsoftProxyApi tinsoftProxyApi;

        /// <summary>
        /// 
        /// </summary>
        public int Location { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public TinsoftProxyApiWrapper(TinsoftProxyApi tinsoftProxyApi)
        {
            this.tinsoftProxyApi = tinsoftProxyApi ?? throw new ArgumentNullException(nameof(tinsoftProxyApi));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public TinsoftProxyApiWrapper(string apiKey)
        {
            this.tinsoftProxyApi = new TinsoftProxyApi(apiKey);
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
        public async Task<IProxyApiResponseWrapper> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            var result = await tinsoftProxyApi.ChangeProxy(Location).ConfigureAwait(false);
            if (!result.Success && result.Description?.Contains("expired") == true)
                throw new InvalidOperationException(result.Description);
            
            return new ProxyApiResponseWrapper()
            {
                IsSuccess = result.Success,
                Proxy = result.Proxy,
                NextTime = DateTime.Now.AddSeconds(result.NextChange),
                ExpiredTime = DateTime.Now.AddSeconds(result.Timeout),
                Message = result.Description
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"TinsoftProxy ({tinsoftProxyApi.ApiKey})";
        }
    }
}
