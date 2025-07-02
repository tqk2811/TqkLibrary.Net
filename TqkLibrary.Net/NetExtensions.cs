using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
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

#if NET5_0_OR_GREATER
        public static SocketsHttpHandler DisableFindIpV6(this SocketsHttpHandler handler)
        {
            handler.ConnectCallback = async (context, cancellationToken) =>
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await socket.ConnectAsync(context.DnsEndPoint.Host, context.DnsEndPoint.Port);
                return new NetworkStream(socket, true);
            };
            return handler;
        }
#endif
    }
}