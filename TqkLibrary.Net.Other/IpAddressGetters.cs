﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Other
{
    public static class IpAddressGetters
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<MyIp> GetCurrentIpAdreess()
        {
            using HttpClient httpClient = new HttpClient(NetSingleton.HttpClientHandler, false);
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.myip.com");
            using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<MyIp>(await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false))!;
        }
    }
}
