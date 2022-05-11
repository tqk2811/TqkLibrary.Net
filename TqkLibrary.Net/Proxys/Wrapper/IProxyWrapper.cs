using System;

namespace TqkLibrary.Net.Proxys.Wrapper
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
        public string Proxy { get; }
    }
}
