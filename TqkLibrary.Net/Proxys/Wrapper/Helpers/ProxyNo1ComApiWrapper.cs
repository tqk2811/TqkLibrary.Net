using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxys.Wrapper.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyNo1ComApiWrapper : IProxyApiWrapper
    {
        readonly ProxyNo1ComApi proxyNo1ComApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyInfo"></param>
        public ProxyNo1ComApiWrapper(ProxyNo1ComKeyInfo keyInfo) : this(new ProxyNo1ComApi(keyInfo))
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxyNo1ComApi"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProxyNo1ComApiWrapper(ProxyNo1ComApi proxyNo1ComApi)
        {
            this.proxyNo1ComApi = proxyNo1ComApi ?? throw new ArgumentNullException(nameof(proxyNo1ComApi));
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAllowGetNewOnUsing => false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IProxyApiResponseWrapper> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            var res_changeIp = await proxyNo1ComApi.ChangeKeyIp(cancellationToken).ConfigureAwait(false);
            var res_status = await proxyNo1ComApi.KeyStatus(cancellationToken).ConfigureAwait(false);
            DateTime? expired_at = res_status.Data.GetExpiredAt;
            return new ProxyApiResponseWrapper()
            {
                IsSuccess = res_changeIp.IsSuccess,
                Proxy = $"{res_status.Data.Proxy.Ip}:{res_status.Data.Proxy.HTTPIPv4}",
                NextTime = DateTime.Now.AddSeconds(res_status.Data.ChangeIpInterval),
                ExpiredTime = expired_at == null ? DateTime.Now.AddHours(1) : expired_at.Value,
                Message = res_changeIp.Message,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return proxyNo1ComApi.ApiKey;
        }
    }
}
