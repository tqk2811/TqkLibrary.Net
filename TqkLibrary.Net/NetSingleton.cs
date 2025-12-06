using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
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

            if (IsBrowserRuntime())
            {
                HttpClientHandler = new WrapperHttpClientHandler();
            }
            else
            {
                HttpClientHandler = new WrapperHttpClientHandler()
                {
                    AllowAutoRedirect = true,
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                };
            }
        }

        internal static bool IsBrowserRuntime()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsBrowser();
#else
            return RuntimeInformation.ProcessArchitecture.ToString().Equals("Wasm", StringComparison.OrdinalIgnoreCase);
#endif

        }


        /// <summary>
        /// 
        /// </summary>
        public static WrapperHttpClientHandler HttpClientHandler { get; }
        internal static readonly JsonSerializerSettings JsonSerializerSettings;
    }
}