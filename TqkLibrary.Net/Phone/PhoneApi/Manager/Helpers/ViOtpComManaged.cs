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
    public class ViOtpComManaged : IPhoneApi
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
        public ViOtpComManaged(ViOtpComApi viOtpComApi)
        {
            this.viOtpComApi = viOtpComApi;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public ViOtpComManaged(string apiKey)
        {
            this.viOtpComApi = new ViOtpComApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        ~ViOtpComManaged()
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
        public async Task<IPhoneSession> RentPhone(CancellationToken cancellationToken = default)
        {
            if (Service == null) throw new InvalidOperationException($"{nameof(Service)} must be set");
            var res = await viOtpComApi.RequestRent(Service, Networks, Prefixs, ExceptPrefixs, cancellationToken);
            return new ViOtpComManagedSession(viOtpComApi, res.Data);
        }
    }

    internal class ViOtpComManagedSession : IPhoneSession
    {
        readonly ViOtpComApi viOtpComApi;
        readonly ViOtpComSession viOtpComSession;
        internal ViOtpComManagedSession(ViOtpComApi viOtpComApi, ViOtpComSession viOtpComSession)
        {
            this.viOtpComApi = viOtpComApi;
            this.viOtpComSession = viOtpComSession;
        }
        public string PhoneNumber => viOtpComSession?.PhoneNumber;

        public Task CancelWaitSms(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<IPhoneSms>> GetSms(CancellationToken cancellationToken = default)
        {
            var res = await viOtpComApi.SessionGet(viOtpComSession).ConfigureAwait(false);
            return new ViOtpComManagedSms[] { new ViOtpComManagedSms(res.Data) };
        }
    }

    internal class ViOtpComManagedSms : IPhoneSms
    {
        readonly ViOtpComSessionGet viOtpComSessionGet;
        internal ViOtpComManagedSms(ViOtpComSessionGet viOtpComSessionGet)
        {
            this.viOtpComSessionGet = viOtpComSessionGet;
        }
        public string Text => viOtpComSessionGet?.SmsContent;

        public string Code => viOtpComSessionGet?.Code;
    }
}
