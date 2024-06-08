using System;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxy.Wrapper
{
    internal class ProxyWrapper : IProxyWrapper
    {
        readonly ProxyApiItemData _proxyApiItemData;
        internal ProxyWrapper(ProxyApiItemData proxyApiItemData)
        {
            this._proxyApiItemData = proxyApiItemData ?? throw new ArgumentNullException(nameof(proxyApiItemData));
            proxyApiItemData.AddRef();
        }
        ~ProxyWrapper()
        {
            _proxyApiItemData.RemoveRef();
        }

        public string Proxy { get { return _proxyApiItemData.CurrentProxy; } }

        public ProxyType ProxyType { get { return _proxyApiItemData.ProxyType; } }


#if NET5_0_OR_GREATER
        public async ValueTask DisposeAsync()
        {
            await _proxyApiItemData.RemoveRefAsync();
            GC.SuppressFinalize(this);
        }
#else
        public void Dispose()
        {
            _proxyApiItemData.RemoveRef();
            GC.SuppressFinalize(this);
        }
#endif
    }
}
