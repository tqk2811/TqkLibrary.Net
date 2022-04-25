using System;

namespace TqkLibrary.Net.Proxys.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get; }
        
        /// <summary>
        /// Next time request
        /// </summary>
        public string Proxy { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime NextTime { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpiredTime { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ProxyApiResponse : IProxyApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Next time request
        /// </summary>
        public string Proxy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime NextTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpiredTime { get; set; }
    }
}
