using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Mail.TempMails.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailWrapperSession : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; }
        /// <summary>
        /// 
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
