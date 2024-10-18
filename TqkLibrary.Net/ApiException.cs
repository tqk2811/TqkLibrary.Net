using System;
using System.Net;
namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiException : Exception
    {
        public ApiException()
        {

        }

        public ApiException(string message) : base(message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode? StatusCode { get; init; }
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
        public ApiException() { }
        public ApiException(string message) : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        public T? Body { get; init; }
    }
}