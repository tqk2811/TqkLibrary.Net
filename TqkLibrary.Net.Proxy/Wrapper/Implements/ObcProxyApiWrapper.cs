using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Proxy.Services;

namespace TqkLibrary.Net.Proxy.Wrapper.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObcProxyApiWrapperExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obcProxyApi"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<ObcProxyApiWrapper>> GetList(this BocProxyApi obcProxyApi, CancellationToken cancellationToken = default)
        {
            var list = await obcProxyApi.ProxyList(cancellationToken).ConfigureAwait(false);
            return list.Select(x => new ObcProxyApiWrapper(obcProxyApi, x));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ObcProxyApiWrapper : IProxyApiWrapper
    {
        readonly BocProxyApi _obcProxyApi;
        readonly BocProxyApi.ObcProxy _obcProxy;

        /// <summary>
        /// 
        /// </summary>
        public ObcProxyApiWrapper(BocProxyApi obcProxyApi, BocProxyApi.ObcProxy obcProxy)
        {
            this._obcProxyApi = obcProxyApi;
            this._obcProxy = obcProxy;
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
        public async Task<IProxyApiResponseWrapper?> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            await _obcProxyApi.Reset(_obcProxy, cancellationToken).ConfigureAwait(false);
            var list = await _obcProxyApi.ProxyList(cancellationToken).ConfigureAwait(false);
            BocProxyApi.ObcProxy? obcProxy = list.FirstOrDefault(x => x.ProxyPort == _obcProxy.ProxyPort);
            return new ProxyApiResponseWrapper()
            {
                IsSuccess = obcProxy is not null,
                Proxy = obcProxy?.GetProxy(),
                NextTime = DateTime.Now.AddDays(-1),
                ExpiredTime = DateTime.Now.AddDays(365),
                ProxyType = ProxyType.Http,
                Message = string.Empty,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ObcProxy ({_obcProxy?.GetProxy()})";
        }
    }
}
