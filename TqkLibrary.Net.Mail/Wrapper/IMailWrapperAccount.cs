using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Mail.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailWrapperAccount : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        string Email { get; }

        /// <summary>
        /// 
        /// </summary>
        string? Password { get; }

        /// <summary>
        /// Login or connect to mail server
        /// </summary>
        /// <returns>Email</returns>
        Task<string> InitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IMailWrapperEmail>> GetMailsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// some temp maill need delete account
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync(CancellationToken cancellationToken = default);
    }
}
