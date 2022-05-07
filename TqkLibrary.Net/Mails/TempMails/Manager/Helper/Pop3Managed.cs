using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Mails.TempMails.Manager.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class Pop3Managed : IMailManaged
    {
        readonly string host;
        readonly int port;
        readonly Queue<ImapAccount> imapAccounts;
        readonly AsyncLock asyncLock = new AsyncLock();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="imapAccounts"></param>
        public Pop3Managed(string host, int port, IEnumerable<ImapAccount> imapAccounts)
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IMailSession> CreateSessionAsync(string login, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailSession"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ReQueueSessionAsync(IMailSession mailSession, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    internal class Pop3Session : IMailSession
    {
        public string Email => throw new NotImplementedException();

        public string Password => throw new NotImplementedException();

        public Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IMail>> GetMailsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> InitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    internal class Pop3Mail : IMail
    {
        public string FromAddress => throw new NotImplementedException();
        public string Subject => throw new NotImplementedException();
        public string RawBody => throw new NotImplementedException();
    }
}
