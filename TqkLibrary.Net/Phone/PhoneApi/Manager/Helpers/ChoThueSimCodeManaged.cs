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
    public class ChoThueSimCodeManaged : IPhoneApi
    {
        readonly ChoThueSimCodeApi choThueSimCodeApi;

        /// <summary>
        /// 
        /// </summary>
        public ChoThueSimAppInfo ChoThueSimAppInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ChoThueSimCarrier? ChoThueSimCarrier { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public ChoThueSimCodeManaged(string apiKey)
        {
            this.choThueSimCodeApi = new ChoThueSimCodeApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="choThueSimCodeApi"></param>
        public ChoThueSimCodeManaged(ChoThueSimCodeApi choThueSimCodeApi)
        {
            this.choThueSimCodeApi = choThueSimCodeApi ?? throw new ArgumentNullException(nameof(choThueSimCodeApi));
        }
        /// <summary>
        /// 
        /// </summary>
        ~ChoThueSimCodeManaged()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        void Dispose(bool disposing)
        {
            choThueSimCodeApi.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IPhoneSession> RentPhone(CancellationToken cancellationToken = default)
        {
            if (ChoThueSimAppInfo == null) throw new InvalidOperationException($"{nameof(ChoThueSimAppInfo)} is null");
            var phone = await choThueSimCodeApi.GetPhoneNumber(ChoThueSimAppInfo, ChoThueSimCarrier, cancellationToken).ConfigureAwait(false);
            return new ChoThueSimCodePhoneSession(choThueSimCodeApi, phone);
        }
    }


    internal class ChoThueSimCodePhoneSession : IPhoneSession
    {
        readonly ChoThueSimCodeApi choThueSimCodeApi;
        readonly ChoThueSimBaseResult<ChoThueSimResponseCodeGetPhoneNumber, ChoThueSimPhoneNumberResult> phone;
        internal ChoThueSimCodePhoneSession(
            ChoThueSimCodeApi choThueSimCodeApi, 
            ChoThueSimBaseResult<ChoThueSimResponseCodeGetPhoneNumber, ChoThueSimPhoneNumberResult> phone)
        {
            this.choThueSimCodeApi = choThueSimCodeApi ?? throw new ArgumentNullException(nameof(choThueSimCodeApi));
            this.phone = phone ?? throw new ArgumentNullException(nameof(phone));
        }

        public string PhoneNumber => phone?.Result?.Number;

        public Task CancelWaitSms(CancellationToken cancellationToken = default)
        {
            return choThueSimCodeApi.CancelGetMessage(phone.Result, cancellationToken);
        }

        public async Task<IEnumerable<IPhoneSms>> GetSms(CancellationToken cancellationToken = default)
        {
            var message = await choThueSimCodeApi.GetMessage(phone.Result, cancellationToken).ConfigureAwait(false);
            return new ChoThueSimCodePhoneSms[] { new ChoThueSimCodePhoneSms(message) };
        }
    }

    internal class ChoThueSimCodePhoneSms : IPhoneSms
    {
        readonly ChoThueSimBaseResult<ChoThueSimResponseCodeMessage, ChoThueSimMessageResult> sms;
        internal ChoThueSimCodePhoneSms(
            ChoThueSimBaseResult<ChoThueSimResponseCodeMessage, ChoThueSimMessageResult> sms)
        {
            this.sms = sms;
        }
        public string Text => sms?.Result?.SMS;

        public string Code => sms?.Result?.Code;
    }
}
