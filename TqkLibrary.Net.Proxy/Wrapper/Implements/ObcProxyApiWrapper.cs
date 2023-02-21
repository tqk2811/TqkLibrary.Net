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
        readonly BocProxyApi obcProxyApi;
        readonly ObcProxy obcProxy;

        /// <summary>
        /// 
        /// </summary>
        public ObcProxyApiWrapper(BocProxyApi obcProxyApi, ObcProxy obcProxy)
        {
            this.obcProxyApi = obcProxyApi;
            this.obcProxy = obcProxy;
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
        public async Task<IProxyApiResponseWrapper> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            await obcProxyApi.Reset(obcProxy, cancellationToken).ConfigureAwait(false);
            var list = await obcProxyApi.ProxyList(cancellationToken).ConfigureAwait(false);
            return new ObcProxyApiResponseWrapper(list.FirstOrDefault(x => x.ProxyPort == obcProxy.ProxyPort));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ObcProxy ({obcProxy?.GetProxy()})";
        }
    }

    internal class ObcProxyApiResponseWrapper : IProxyApiResponseWrapper
    {
        readonly ObcProxy obcProxy;
        internal ObcProxyApiResponseWrapper(ObcProxy obcProxy)
        {
            this.obcProxy = obcProxy;
        }
        public bool IsSuccess => true;
        public string Proxy => obcProxy?.GetProxy();
        public DateTime NextTime => DateTime.Now.AddDays(-1);
        public DateTime ExpiredTime => DateTime.Now.AddDays(365);
        public string Message => string.Empty;
    }
}
