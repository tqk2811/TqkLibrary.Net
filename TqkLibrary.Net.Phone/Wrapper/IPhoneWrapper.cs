using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Phone.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPhoneWrapper : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IPhoneWrapperSession> RentPhoneAsync(CancellationToken cancellationToken = default);
    }
}
