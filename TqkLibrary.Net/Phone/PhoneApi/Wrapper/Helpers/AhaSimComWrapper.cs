using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Phone.PhoneApi.Wrapper.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class AhaSimComWrapper : IPhoneWrapper
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
        public AhaSimComWrapper(AhaSimComApi ahaSimComApi)
        {
            this.ahaSimComApi = ahaSimComApi;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public AhaSimComWrapper(string apiKey)
        {
            this.ahaSimComApi = new AhaSimComApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        ~AhaSimComWrapper()
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
        public async Task<IPhoneWrapperSession> RentPhone(CancellationToken cancellationToken = default)
        {
            if (AhaSimComService == null) throw new InvalidOperationException($"Set value to {nameof(AhaSimComService)} first");
            var session = await ahaSimComApi.PhoneNewSession(AhaSimComService, Networks, Prefixs, ExceptPrefixs, cancellationToken).ConfigureAwait(false);
            if (session.Success)
                return new AhaSimComWrapperSession(session.Data, ahaSimComApi);
            else
                return null;
        }
    }

    internal class AhaSimComWrapperSession : IPhoneWrapperSession
    {
        readonly AhaSimComSession ahaSimComSession;
        readonly AhaSimComApi ahaSimComApi;
        internal AhaSimComWrapperSession(AhaSimComSession ahaSimComSession, AhaSimComApi ahaSimComApi)
        {
            this.ahaSimComSession = ahaSimComSession;
            this.ahaSimComApi = ahaSimComApi;
        }

        public string PhoneNumber => ahaSimComSession?.PhoneNumber;

        public Task CancelWaitSms(CancellationToken cancellationToken = default)
        {
            return ahaSimComApi.SessionCancel(ahaSimComSession, cancellationToken);
        }

        public async Task<IEnumerable<IPhoneWrapperSms>> GetSms(CancellationToken cancellationToken = default)
        {
            var messages = await ahaSimComApi.SessionGetOtp(ahaSimComSession, cancellationToken).ConfigureAwait(false);
            return new AhaSimComWrapperSms[] { new AhaSimComWrapperSms(messages.Data) };
        }
    }

    internal class AhaSimComWrapperSms : IPhoneWrapperSms
    {
        readonly AhaSimComOtp ahaSimComOtp;
        internal AhaSimComWrapperSms(AhaSimComOtp ahaSimComOtp)
        {
            this.ahaSimComOtp = ahaSimComOtp;
        }
        public string Text => ahaSimComOtp?.Message?.Content;

        public string Code => ahaSimComOtp?.Message?.Otp;
    }
}
