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
    public class ViOtpComWrapper : IPhoneWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public ViOtpComService Service { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ViOtpComNetwork> Networks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Prefixs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> ExceptPrefixs { get; set; }

        readonly ViOtpComApi viOtpComApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viOtpComApi"></param>
        public ViOtpComWrapper(ViOtpComApi viOtpComApi)
        {
            this.viOtpComApi = viOtpComApi;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public ViOtpComWrapper(string apiKey)
        {
            this.viOtpComApi = new ViOtpComApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        ~ViOtpComWrapper()
        {
            viOtpComApi.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            viOtpComApi.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPhoneWrapperSession> RentPhone(CancellationToken cancellationToken = default)
        {
            if (Service == null) throw new InvalidOperationException($"{nameof(Service)} must be set");
            var res = await viOtpComApi.RequestRent(Service, Networks, Prefixs, ExceptPrefixs, cancellationToken);
            return new ViOtpComWrapperSession(viOtpComApi, res.Data);
        }
    }

    internal class ViOtpComWrapperSession : IPhoneWrapperSession
    {
        readonly ViOtpComApi viOtpComApi;
        readonly ViOtpComSession viOtpComSession;
        internal ViOtpComWrapperSession(ViOtpComApi viOtpComApi, ViOtpComSession viOtpComSession)
        {
            this.viOtpComApi = viOtpComApi;
            this.viOtpComSession = viOtpComSession;
        }
        public string PhoneNumber => viOtpComSession?.PhoneNumber;

        public Task CancelWaitSms(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<IPhoneWrapperSms>> GetSms(CancellationToken cancellationToken = default)
        {
            var res = await viOtpComApi.SessionGet(viOtpComSession).ConfigureAwait(false);
            return new ViOtpComWrapperSms[] { new ViOtpComWrapperSms(res.Data) };
        }
    }

    internal class ViOtpComWrapperSms : IPhoneWrapperSms
    {
        readonly ViOtpComSessionGet viOtpComSessionGet;
        internal ViOtpComWrapperSms(ViOtpComSessionGet viOtpComSessionGet)
        {
            this.viOtpComSessionGet = viOtpComSessionGet;
        }
        public string Text => viOtpComSessionGet?.SmsContent;

        public string Code => viOtpComSessionGet?.Code;
    }
}
