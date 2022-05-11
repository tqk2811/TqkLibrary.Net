using System;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxys.Wrapper
{
    internal class ProxyWrapper : IProxyWrapper
    {
        readonly ProxyApiItemData proxyApiItemData;
        internal ProxyWrapper(ProxyApiItemData proxyApiItemData)
        {
            this.Proxy = proxyApiItemData.CurrentProxy;
            this.proxyApiItemData = proxyApiItemData;
            proxyApiItemData.AddRef();
        }
        ~ProxyWrapper()
        {
            proxyApiItemData.RemoveRef();
        }

        public string Proxy { get; }


#if NET5_0_OR_GREATER
        public async ValueTask DisposeAsync()
        {
            await proxyApiItemData.RemoveRefAsync();
            GC.SuppressFinalize(this);
        }
#else
        public void Dispose()
        {
            proxyApiItemData.RemoveRef();
            GC.SuppressFinalize(this);
        }
#endif
    }
}
