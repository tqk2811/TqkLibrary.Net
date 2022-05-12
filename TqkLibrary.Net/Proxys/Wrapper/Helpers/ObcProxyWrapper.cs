using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Proxys.Wrapper.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ObcProxyWrapper : IProxyApiWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsAllowGetNewOnUsing => false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IProxyApiResponseWrapper> GetNewProxyAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
