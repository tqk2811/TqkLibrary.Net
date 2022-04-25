using System;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxys.Manager
{
    internal class Proxy_ : IProxy
    {
        readonly ProxyApiItemData proxyApiItemData;
        internal Proxy_(ProxyApiItemData proxyApiItemData)
        {
            this.Proxy = proxyApiItemData.CurrentProxy;
            this.proxyApiItemData = proxyApiItemData;
            proxyApiItemData.AddRef();
        }
        ~Proxy_()
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
