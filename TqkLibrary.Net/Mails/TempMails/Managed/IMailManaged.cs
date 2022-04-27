using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Mails.TempMails.Managed
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailManaged
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IMailSession> CreateTempMailSessionAsync(string login, CancellationToken cancellationToken = default);
        
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IMailSession : IDisposable
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
        Task<IEnumerable<IMail>> GetMailsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// some temp maill need delete account
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IMail
    {
        /// <summary>
        /// 
        /// </summary>
        public string FromAddress { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Subject { get; }
        /// <summary>
        /// 
        /// </summary>
        public string RawBody { get; }
    }
}
