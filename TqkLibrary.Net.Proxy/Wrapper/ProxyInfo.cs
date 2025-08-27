﻿using System;
using TqkLibrary.Net.Proxy.Wrapper.Enums;
using TqkLibrary.Net.Proxy.Wrapper.Interfaces;

namespace TqkLibrary.Net.Proxy.Wrapper
{
    public class ProxyInfo : IProxyInfo
    {
        public required string Address { get; set; }

        public required int Port { get; set; }

        public required ProxyType ProxyType { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, this))
                return true;

            if (obj is ProxyInfo other)
            {
                return this.Address.Equals(other.Address, StringComparison.OrdinalIgnoreCase)
                    && this.Port == other.Port
                    && this.ProxyType == other.ProxyType;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Address);
            hashCode.Add(Port);
            hashCode.Add(ProxyType);
            return hashCode.ToHashCode();
        }

        public static ProxyInfo? ParseHttpProxy(string? httpProxy, char split = ':')
        {
            if (string.IsNullOrWhiteSpace(httpProxy))
                return null;
            var spliteds = httpProxy!.Split(split);
            if (spliteds.Length >= 2 && int.TryParse(spliteds[1], out int port))
            {
                ProxyInfo proxyInfo = new ProxyInfo()
                {
                    Address = spliteds[0],
                    Port = port,
                    ProxyType = ProxyType.Http,
                };
                if (spliteds.Length >= 4)
                {
                    proxyInfo.UserName = spliteds[2];
                    proxyInfo.Password = spliteds[3];
                }
                return proxyInfo;
            }
            return null;
        }
    }
}
