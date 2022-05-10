using Newtonsoft.Json;
using System.Net.Http;

namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public static class NetSingleton
    {
        internal static readonly HttpClientHandler clientHandler = new HttpClientHandler()
        {
            UseCookies = true,
            CookieContainer = new System.Net.CookieContainer(),
            AllowAutoRedirect = true,
            AutomaticDecompression = System.Net.DecompressionMethods.GZip
        };
        internal static readonly HttpClient httpClient = new HttpClient(clientHandler);


        internal static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new IgnoreStringEmptyContractResolver()
        };
    }
}