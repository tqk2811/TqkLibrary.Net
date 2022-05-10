using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Phone.PhoneApi.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPhoneApi : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IPhoneSession> RentPhone(CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public interface IPhoneSession
    {
        /// <summary>
        /// 
        /// </summary>
        string PhoneNumber { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IPhoneSms>> GetSms(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task CancelWaitSms(CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public interface IPhoneSms
    {
        /// <summary>
        /// 
        /// </summary>
        string Text { get; }

        /// <summary>
        /// 
        /// </summary>
        string Code { get; }
    }
}
