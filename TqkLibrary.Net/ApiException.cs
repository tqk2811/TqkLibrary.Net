using System;
using System.Net;
namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(ApiException)}: {StatusCode}";
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiException<T> : ApiException
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiException()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public T Body { get; set; }
    }
}