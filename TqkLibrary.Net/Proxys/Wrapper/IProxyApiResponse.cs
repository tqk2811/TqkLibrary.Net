using System;

namespace TqkLibrary.Net.Proxys.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyApiResponseWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsSuccess { get; }
        
        /// <summary>
        /// Next time request
        /// </summary>
        string Proxy { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime NextTime { get; }
        
        /// <summary>
        /// 
        /// </summary>
        DateTime ExpiredTime { get; }

        /// <summary>
        /// 
        /// </summary>
        string Message { get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ProxyApiResponseWrapper : IProxyApiResponseWrapper
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

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
}
