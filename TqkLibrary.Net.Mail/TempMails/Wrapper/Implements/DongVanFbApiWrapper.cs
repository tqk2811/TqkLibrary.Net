using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.Mail.BuyMailApi;

namespace TqkLibrary.Net.Mail.TempMails.Wrapper.Implements
{
    /// <summary>
    /// 
    /// </summary>
    public class DongVanFbApiWrapper : IMailWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public DongVanFbAccountType AccountType { get; set; }
        /// <summary>
        /// Default 1
        /// </summary>
        public int Amount { get; } = 1;

        readonly DongVanFbApi dongVanFbApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dongVanFbApi"></param>
        public DongVanFbApiWrapper(DongVanFbApi dongVanFbApi)
        {
            this.dongVanFbApi = dongVanFbApi;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public DongVanFbApiWrapper(string apiKey)
        {
            this.dongVanFbApi = new DongVanFbApi(apiKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IMailWrapperSession> CreateSessionAsync(CancellationToken cancellationToken = default)
        {
            if (AccountType == null) throw new InvalidOperationException($"{AccountType} was null");
            var acc = await dongVanFbApi.BuyMail(AccountType, Amount, cancellationToken).ConfigureAwait(false);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailSession"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ReQueueSessionAsync(IMailWrapperSession mailSession, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }



        internal class DongVanFbApiWrapperSession : IMailWrapperSession
        {
            readonly DongVanFbApi _dongVanFbApi;
            readonly DongVanFbBuyMailResponse _dongVanFbBuyMailResponse;
            readonly DongVanFbMailAccount _account;
            internal DongVanFbApiWrapperSession(DongVanFbApi dongVanFbApi, DongVanFbBuyMailResponse dongVanFbBuyMailResponse)
            {
                this._dongVanFbApi = dongVanFbApi;
                this._dongVanFbBuyMailResponse = dongVanFbBuyMailResponse;
                this._account = dongVanFbBuyMailResponse.Data.ListDataAccount.FirstOrDefault();
            }
            public string Email => _account?.Email;
            public string Password => _account?.Password;

            public Task DeleteAsync(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public void Dispose()
            {

            }

            public async Task<IEnumerable<IMailWrapperEmail>> GetMailsAsync(CancellationToken cancellationToken = default)
            {
                var messages = await _dongVanFbApi.GetMessages(_account, cancellationToken).ConfigureAwait(false);
                return messages.Messages.Select(x => new DongVanFbApiWrapperEmail(x));
            }

            public Task<string> InitAsync(CancellationToken cancellationToken = default)
            {
                return Task.FromResult(string.Empty);
            }
        }

        internal class DongVanFbApiWrapperEmail : IMailWrapperEmail
        {
            readonly DongVanFbMessage _dongVanFbMessage;
            internal DongVanFbApiWrapperEmail(DongVanFbMessage dongVanFbMessage)
            {
                this._dongVanFbMessage = dongVanFbMessage;
            }
            public string FromAddress => _dongVanFbMessage?.From?.FirstOrDefault()?.Address;

            public string Subject => _dongVanFbMessage?.Subject;

            public string RawBody => _dongVanFbMessage?.Message;

            public string Code => _dongVanFbMessage?.Code;
        }
    }
}
