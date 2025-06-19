using System;
using TqkLibrary.Net.Proxy.Wrapper.Enums;
using TqkLibrary.Net.Proxy.Wrapper.Interfaces;

namespace TqkLibrary.Net.Proxy.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyApiResponseWrapper : IProxyApiResponseWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get; set; }

        IProxyInfo? IProxyApiResponseWrapper.Proxy => this.Proxy;
        /// <summary>
        /// 
        /// </summary>
        public ProxyInfo? Proxy { get; set; }

        /// <summary>
        /// Next time request
        /// </summary>
        public DateTime? NextTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ExpiredTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Message { get; set; }

    }
}
