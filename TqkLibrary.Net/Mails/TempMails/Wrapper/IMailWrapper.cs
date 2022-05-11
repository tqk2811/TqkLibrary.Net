using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Mails.TempMails.Wrapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IMailWrapperSession> CreateSessionAsync(string login, CancellationToken cancellationToken = default);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ReQueueSessionAsync(IMailWrapperSession mailSession, CancellationToken cancellationToken = default);
    }
}
