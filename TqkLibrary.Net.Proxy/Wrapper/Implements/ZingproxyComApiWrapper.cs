using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Proxy.Services;
using TqkLibrary.Net.Proxy.Wrapper.Enums;
using TqkLibrary.Net.Proxy.Wrapper.Interfaces;

namespace TqkLibrary.Net.Proxy.Wrapper.Implements
{
    public class ZingproxyComApiWrapper : IProxyApiWrapper
    {
        /// <summary>
        /// socks5 or http proxy
        /// </summary>
        public ProxyType ProxyType { get; set; } = ProxyType.Http;
        public bool IsAllowGetNewOnUsing => false;


        readonly ZingproxyComApi _zingproxyComApi;
        public ZingproxyComApiWrapper(ZingproxyComApi zingproxyComApi)
        {
            this._zingproxyComApi = zingproxyComApi;
        }
        public ZingproxyComApiWrapper(string apiKey) : this(new ZingproxyComApi(apiKey))
        {

        }


        public virtual async Task<IProxyApiResponseWrapper?> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            var res = await _zingproxyComApi.GetProxy(cancellationToken);

            ProxyApiResponseWrapper result = new ProxyApiResponseWrapper()
            {
                IsSuccess = res.Status == ZingproxyComApi.ResponseStatus.Success,
                Message = res.Error
            };
            if (result.IsSuccess)
            {
                result.NextTime = DateTime.Now;
                result.ExpiredTime = res.Proxy?.DateEnd;
                if (ProxyType.HasFlag(ProxyType.Http) && !string.IsNullOrWhiteSpace(res.Proxy?.HttpProxy))
                {
                    result.Proxy = ProxyInfo.ParseHttpProxy(res.Proxy!.HttpProxy);
                }
                else if (ProxyType.HasFlag(ProxyType.Socks5) && !string.IsNullOrWhiteSpace(res.Proxy?.Socks5Proxy))
                {
                    result.Proxy = ProxyInfo.ParseHttpProxy(res.Proxy!.Socks5Proxy);
                    if (result.Proxy is not null)
                    {
                        result.Proxy.ProxyType = ProxyType.Socks5;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(res.Proxy?.HttpProxy))
                {
                    result.Proxy = ProxyInfo.ParseHttpProxy(res.Proxy!.HttpProxy);
                }
                else if (!string.IsNullOrWhiteSpace(res.Proxy?.Socks5Proxy))
                {
                    result.Proxy = ProxyInfo.ParseHttpProxy(res.Proxy!.Socks5Proxy);
                    if (result.Proxy is not null)
                    {
                        result.Proxy.ProxyType = ProxyType.Socks5;
                    }
                }

                result.IsSuccess = result.Proxy is not null;
            }
            return result;
        }
    }
}
