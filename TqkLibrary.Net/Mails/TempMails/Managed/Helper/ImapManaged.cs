using MailKit.Net.Imap;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
using MailKit.Security;
using MailKit;
using MailKit.Search;

namespace TqkLibrary.Net.Mails.TempMails.Managed.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class ImapAccount
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ImapManaged : IMailManaged
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
        public ImapManaged(string host, int port, IEnumerable<ImapAccount> imapAccounts)
        {
            this.host = host;
            this.port = port;
            this.imapAccounts = new Queue<ImapAccount>(imapAccounts);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IMailSession> CreateTempMailSessionAsync(string login, CancellationToken cancellationToken = default)
        {
            using (await asyncLock.LockAsync(cancellationToken))
            {
                if (imapAccounts.Count == 0) return null;
                var account = imapAccounts.Dequeue();
                return new ImapSession(host, port, account);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ImapSession : IMailSession
    {
        readonly string host;
        readonly int port;
        readonly ImapAccount account;
        internal ImapSession(string host, int port, ImapAccount imapAccount)
        {
            this.host = host;
            this.port = port;
            this.account = imapAccount;
        }

        readonly ImapClient imapClient = new ImapClient();
        /// <summary>
        /// 
        /// </summary>
        public string Email => account.UserName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            return imapClient.Inbox.CloseAsync(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            imapClient.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<IMail>> GetMailsAsync(CancellationToken cancellationToken = default)
        {
            var uids = await imapClient.Inbox.SearchAsync(SearchQuery.All, cancellationToken).ConfigureAwait(false);
            var results = new List<IMail>();
            foreach (var uid in uids)
            {
                var message = await imapClient.Inbox.GetMessageAsync(uid, cancellationToken).ConfigureAwait(false);
                results.Add(new IMapMail(message));
            }
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> InitAsync(CancellationToken cancellationToken = default)
        {
            await imapClient.ConnectAsync(host, port, SecureSocketOptions.Auto, cancellationToken).ConfigureAwait(false);
            await imapClient.AuthenticateAsync(account.UserName, account.Password, cancellationToken).ConfigureAwait(false);
            await imapClient.Inbox.OpenAsync(FolderAccess.ReadOnly, cancellationToken).ConfigureAwait(false);
            return account.UserName;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IMapMail : IMail
    {
        readonly MimeMessage mimeMessage;
        internal IMapMail(MimeMessage mimeMessage)
        {
            this.mimeMessage = mimeMessage ?? throw new ArgumentNullException(nameof(mimeMessage));
        }
        /// <summary>
        /// 
        /// </summary>
        public string FromAddress => mimeMessage.From?.First()?.Name;
        /// <summary>
        /// 
        /// </summary>
        public string Subject => mimeMessage.Subject;
        /// <summary>
        /// 
        /// </summary>
        public string RawBody => mimeMessage.HtmlBody;
    }
}
