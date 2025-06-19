using System;
using TqkLibrary.Net.Proxy.Wrapper.Enums;

namespace TqkLibrary.Net.Proxy.Wrapper.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyWrapper : IDisposable
#if NET5_0_OR_GREATER
        , IAsyncDisposable
#endif
    {
        IProxyInfo? ProxyInfo { get; }
    }
}
