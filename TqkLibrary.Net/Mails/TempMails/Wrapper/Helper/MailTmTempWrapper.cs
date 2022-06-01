using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Mails.TempMails.Wrapper.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class MailTmTempWrapper : IMailWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IMailWrapperSession> CreateSessionAsync(string login, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(login)) throw new ArgumentNullException(nameof(login));
            return Task.FromResult<IMailWrapperSession>(new MailTmTempManagedSession(login));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailSession"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ReQueueSessionAsync(IMailWrapperSession mailSession, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }


    internal class MailTmTempManagedSession : IMailWrapperSession
    {
        static readonly Random random = new Random();
        readonly MailTmApi mailTmApi = new MailTmApi();
        readonly string login;
        MailTmToken token;
        MailTmAccount mailTmAccount;
        MailTmAccountResponse mailTmAccountResponse;
        readonly Dictionary<string, MailTmMessageData> dict_messageDatas = new Dictionary<string, MailTmMessageData>();

        internal MailTmTempManagedSession(string login)
        {
            this.login = login;
        }
        ~MailTmTempManagedSession()
        {
            mailTmApi.Dispose();
        }
        public void Dispose()
        {
            mailTmApi.Dispose();
            GC.SuppressFinalize(this);
        }
        public string Email => mailTmAccount?.Address;

        public string Password => string.Empty;

        public async Task<IEnumerable<IMailWrapperEmail>> GetMailsAsync(CancellationToken cancellationToken = default)
        {
            if (token == null) throw new InvalidOperationException();

            var messages = await mailTmApi.Messages(token, 1, cancellationToken).ConfigureAwait(false);
            var messageDatas = new List<MailTmMessageData>();
            foreach (var message in messages.Members.Where(x => !dict_messageDatas.ContainsKey(x.Id)))
            {
                messageDatas.Add(await mailTmApi.Message(token, message, cancellationToken).ConfigureAwait(false));
                dict_messageDatas[message.Id] = messageDatas.Last();
            }
            return messages.Members.Select(x => new MailTmTempManagedMail(dict_messageDatas[x.Id]));
        }

        public async Task<string> InitAsync(CancellationToken cancellationToken = default)
        {
            if (token != null) throw new InvalidOperationException();
            var domains = await mailTmApi.Domains().ConfigureAwait(false);
            var domain = domains.Members[random.Next(domains.Members.Count)];
            mailTmAccount = new MailTmAccount(login, domain, "sdfw2szfw3qr");
            mailTmAccountResponse = await mailTmApi.AccountCreate(mailTmAccount, cancellationToken).ConfigureAwait(false);
            token = await mailTmApi.Token(mailTmAccount, cancellationToken).ConfigureAwait(false);
            return mailTmAccount.Address;
        }

        public async Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            if (token == null || mailTmAccountResponse == null) throw new InvalidOperationException();
            await mailTmApi.AccountsDelete(mailTmAccountResponse, token, cancellationToken).ConfigureAwait(false);
        }
    }

    internal class MailTmTempManagedMail : IMailWrapperEmail
    {
        readonly MailTmMessageData mailTmMessageData;
        public MailTmTempManagedMail(MailTmMessageData mailTmMessageData)
        {
            this.mailTmMessageData = mailTmMessageData ?? throw new ArgumentNullException(nameof(mailTmMessageData));
        }

        public string FromAddress
        {
            get
            {
                return mailTmMessageData.From?.Address;
            }
        }

        public string Subject
        {
            get
            {
                return mailTmMessageData.Subject;
            }
        }

        public string RawBody
        {
            get
            {
                return string.Join("\r\n\r\n", mailTmMessageData.Html);
            }
        }
        public string Code => string.Empty;
    }
}
