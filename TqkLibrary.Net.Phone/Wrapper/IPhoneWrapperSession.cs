using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Phone.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPhoneWrapperSession
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// 
        /// </summary>
        string PhoneNumber { get; }

        /// <summary>
        /// 
        /// </summary>
        string Message { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IPhoneWrapperSmsResult<IPhoneWrapperSms>> GetSmsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task CancelWaitSmsAsync(CancellationToken cancellationToken = default);
    }
}
