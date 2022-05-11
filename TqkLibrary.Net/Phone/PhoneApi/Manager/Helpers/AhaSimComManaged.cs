using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Phone.PhoneApi.Manager.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class AhaSimComManaged : IPhoneApi
    {
        /// <summary>
        /// 
        /// </summary>
        public AhaSimComService AhaSimComService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AhaSimComNetwork> Networks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Prefixs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> ExceptPrefixs { get; set; }


        readonly AhaSimComApi ahaSimComApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ahaSimComApi"></param>
        public AhaSimComManaged(AhaSimComApi ahaSimComApi)
        {
            this.ahaSimComApi = ahaSimComApi;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public AhaSimComManaged(string apiKey)
        {
            this.ahaSimComApi = new AhaSimComApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        ~AhaSimComManaged()
        {
            ahaSimComApi.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            ahaSimComApi.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IPhoneSession> RentPhone(CancellationToken cancellationToken = default)
        {
            if (AhaSimComService == null) throw new InvalidOperationException($"Set value to {nameof(AhaSimComService)} first");
            var session = await ahaSimComApi.PhoneNewSession(AhaSimComService, Networks, Prefixs, ExceptPrefixs, cancellationToken).ConfigureAwait(false);
            if (session.Success)
                return new AhaSimComManagedSession(session.Data, ahaSimComApi);
            else
                return null;
        }
    }

    internal class AhaSimComManagedSession : IPhoneSession
    {
        readonly AhaSimComSession ahaSimComSession;
        readonly AhaSimComApi ahaSimComApi;
        internal AhaSimComManagedSession(AhaSimComSession ahaSimComSession, AhaSimComApi ahaSimComApi)
        {
            this.ahaSimComSession = ahaSimComSession;
            this.ahaSimComApi = ahaSimComApi;
        }

        public string PhoneNumber => ahaSimComSession?.PhoneNumber;

        public Task CancelWaitSms(CancellationToken cancellationToken = default)
        {
            return ahaSimComApi.SessionCancel(ahaSimComSession, cancellationToken);
        }

        public async Task<IEnumerable<IPhoneSms>> GetSms(CancellationToken cancellationToken = default)
        {
            var messages = await ahaSimComApi.SessionGetOtp(ahaSimComSession, cancellationToken).ConfigureAwait(false);
            return new AhaSimComManagedSms[] { new AhaSimComManagedSms(messages.Data) };
        }
    }

    internal class AhaSimComManagedSms : IPhoneSms
    {
        readonly AhaSimComOtp ahaSimComOtp;
        internal AhaSimComManagedSms(AhaSimComOtp ahaSimComOtp)
        {
            this.ahaSimComOtp = ahaSimComOtp;
        }
        public string Text => ahaSimComOtp?.Message?.Content;

        public string Code => ahaSimComOtp?.Message?.Otp;
    }
}
