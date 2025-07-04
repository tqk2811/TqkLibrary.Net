﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Proxy.Services;
using TqkLibrary.Net.Proxy.Wrapper.Enums;
using TqkLibrary.Net.Proxy.Wrapper.Interfaces;
namespace TqkLibrary.Net.Proxy.Wrapper.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public class TinsoftProxyApiWrapper : IProxyApiWrapper
    {
        readonly TinsoftProxyApi tinsoftProxyApi;

        /// <summary>
        /// 
        /// </summary>
        public int Location { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public TinsoftProxyApiWrapper(TinsoftProxyApi tinsoftProxyApi)
        {
            this.tinsoftProxyApi = tinsoftProxyApi ?? throw new ArgumentNullException(nameof(tinsoftProxyApi));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public TinsoftProxyApiWrapper(string apiKey)
        {
            this.tinsoftProxyApi = new TinsoftProxyApi(apiKey);
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsAllowGetNewOnUsing => true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IProxyApiResponseWrapper?> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            var result = await tinsoftProxyApi.ChangeProxy(Location).ConfigureAwait(false);
            if (!result.Success && result.Description?.Contains("expired") == true)
                throw new InvalidOperationException(result.Description);

            ProxyApiResponseWrapper responseWrapper = new ProxyApiResponseWrapper()
            {
                IsSuccess = result.Success,
                NextTime = DateTime.Now.AddSeconds(result.NextChange),
                ExpiredTime = DateTime.Now.AddSeconds(result.Timeout),
                Message = result.Description,
            };
            if(responseWrapper.IsSuccess)
            {
                responseWrapper.Proxy = ProxyInfo.ParseHttpProxy(result.Proxy);
                responseWrapper.IsSuccess = responseWrapper.Proxy is not null;
            }
            return responseWrapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"TinsoftProxy ({tinsoftProxyApi.ApiKey})";
        }
    }
}
