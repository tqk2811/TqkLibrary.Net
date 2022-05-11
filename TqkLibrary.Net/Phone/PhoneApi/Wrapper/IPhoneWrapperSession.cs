using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Phone.PhoneApi.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPhoneWrapperSession
    {
        /// <summary>
        /// 
        /// </summary>
        string PhoneNumber { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IPhoneWrapperSms>> GetSms(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task CancelWaitSms(CancellationToken cancellationToken = default);
    }
}
