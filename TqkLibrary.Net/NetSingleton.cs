﻿using Newtonsoft.Json;
using TqkLibrary.Net.HttpClientHandles;

namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public static class NetSingleton
    {
        static NetSingleton()
        {
            JsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new MyContractResolver(),
#if DEBUG
                MissingMemberHandling = MissingMemberHandling.Error,
#endif
            };

            HttpClientHandler = new WrapperHttpClientHandler()
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
            };
        }



        /// <summary>
        /// 
        /// </summary>
        public static WrapperHttpClientHandler HttpClientHandler { get; }
        internal static readonly JsonSerializerSettings JsonSerializerSettings;
    }
}