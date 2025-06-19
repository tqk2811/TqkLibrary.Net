using System;
using System.Collections.Generic;
using System.Text;
using TqkLibrary.Net.Proxy.Wrapper.Enums;

namespace TqkLibrary.Net.Proxy.Wrapper.Interfaces
{
    public interface IProxyInfo
    {
        /// <summary>
        /// host or ip
        /// </summary>
        string Address { get; }
        int Port { get; }

        ProxyType ProxyType { get; }

        string? UserName { get; }
        string? Password { get; }
    }
}
