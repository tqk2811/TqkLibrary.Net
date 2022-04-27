using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Mails.TempMails.Managed.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class TempMailOrgManaged : IMailManaged
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IMailSession> CreateTempMailSessionAsync(string login, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IMailSession>(new TempMailOrgSession());
        }
    }

    internal class TempMailOrgSession : IMailSession
    {
        readonly TempMailOrg tempMailOrg = new TempMailOrg();
        TempMailOrgToken token;
        internal TempMailOrgSession()
        {

        }

        public string Email => token.MailBox;

        public string Password => string.Empty;

        public Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            tempMailOrg.Dispose();
        }

        public async Task<IEnumerable<IMail>> GetMailsAsync(CancellationToken cancellationToken = default)
        {
            var messages = await tempMailOrg.Messages(token, cancellationToken).ConfigureAwait(false);
            var mesageDatas = new List<TempMailOrgMessageData>();
            foreach (var item in messages.Messages)
            {
                mesageDatas.Add(await tempMailOrg.MessageData(token, item, cancellationToken).ConfigureAwait(false));
            }
            return mesageDatas.Select(x => new TempMailOrgMail(x));
        }

        public async Task<string> InitAsync(CancellationToken cancellationToken = default)
        {
            token = await tempMailOrg.InitToken(cancellationToken).ConfigureAwait(false);
            return token.MailBox;
        }
    }
    internal class TempMailOrgMail : IMail
    {
        readonly TempMailOrgMessageData messageData;
        internal TempMailOrgMail(TempMailOrgMessageData tempMailOrgMessageData)
        {
            this.messageData = tempMailOrgMessageData ?? throw new ArgumentNullException(nameof(tempMailOrgMessageData));
        }
        public string FromAddress => messageData.From;

        public string Subject => messageData.Subject;

        public string RawBody => messageData.BodyHtml;
    }
}
