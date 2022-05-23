using Newtonsoft.Json;
using System.Net.Http;

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
                ContractResolver = new IgnoreStringEmptyContractResolver()
            };


            clientHandler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = new System.Net.CookieContainer(),
                AllowAutoRedirect = true,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
            };
            httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Connection.Add("Keep-Alive");
        }




        internal static readonly HttpClientHandler clientHandler;
        internal static readonly HttpClient httpClient;
        internal static readonly JsonSerializerSettings JsonSerializerSettings;
    }
}