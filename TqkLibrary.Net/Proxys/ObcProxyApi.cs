using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace TqkLibrary.Net.Proxys
{
    /// <summary>
    /// 
    /// </summary>
    public class ObcProxyApi : BaseApi
    {
        readonly Uri _endPoint;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ObcProxyApi(Uri endpoint)
        {
            this._endPoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<ObcProxy>> ProxyList(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(_endPoint.AbsolutePath, "proxy_list"))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<List<ObcProxy>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obcProxy"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Reset(ObcProxy obcProxy, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(_endPoint.AbsolutePath, "reset").WithParam("proxy", obcProxy.Port))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ResetAll(CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(_endPoint.AbsolutePath, "reset_all"))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obcProxy"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ObcStatus> Status(ObcProxy obcProxy, CancellationToken cancellationToken = default)
            => Build()
            .WithUrlGet(new UriBuilder(_endPoint.AbsolutePath, "status").WithParam("proxy", obcProxy.Port))
            .WithCancellationToken(cancellationToken)
            .ExecuteAsync<ObcStatus>();

        
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class ObcStatus
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("status")]
        public bool Status { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ObcProxy
    {
        /// <summary>
        /// 
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }
}
