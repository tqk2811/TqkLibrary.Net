using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxys.Manager
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
        public Task<IProxyApiResponse> GetNewProxyAsync(CancellationToken cancellationToken);
        /// <summary>
        /// true for allow get new proxy/reset api when some one are using it.<br>
        /// </br>Example: dcom proxy hub, reset will make current device using proxy disconnect. 
        /// </summary>
        public bool IsAllowGetNewOnUsing { get; }

    }
}
