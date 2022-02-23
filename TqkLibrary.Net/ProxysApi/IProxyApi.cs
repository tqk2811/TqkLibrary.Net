using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqkLibrary.Net.ProxysApi
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IProxyApiResponse> GetProxy();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IProxyApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string Proxy { get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime NextRequest { get; }
    }

    internal class ProxyApiResponse : IProxyApiResponse
    {
        public string Proxy { get; set; }

        public DateTime NextRequest { get; set; }
    }
}
