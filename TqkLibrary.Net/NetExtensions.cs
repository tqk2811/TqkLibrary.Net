using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public static class NetExtensions
    {
        /// <summary>
        /// remove path, query of url
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Uri GetDomain(this Uri uri) => new Uri($"{uri.Scheme}://{uri.Authority}");
    }
}