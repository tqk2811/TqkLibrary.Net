using System;

namespace TqkLibrary.Net.Proxy.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyWrapper
#if NET5_0_OR_GREATER
        : IAsyncDisposable
#else
        : IDisposable
#endif
    {
        /// <summary>
        /// 
        /// </summary>
        public ProxyType ProxyType { get; }
        /// <summary>
        /// ip:port or host:port
        /// </summary>
        public string Proxy { get; }
    }
}
