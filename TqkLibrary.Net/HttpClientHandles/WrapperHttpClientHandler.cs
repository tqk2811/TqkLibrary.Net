using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TqkLibrary.Net.HttpClientHandles
{
    /// <summary>
    /// 
    /// </summary>
    public class WrapperHttpClientHandler : HttpClientHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        /// <exception cref="InvalidOperationException"></exception>
        protected override void Dispose(bool disposing)
        {
            throw new InvalidOperationException("WrapperHttpClientHandler can't dispose");
        }
    }
}
