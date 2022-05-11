﻿using System;
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
    public class ChoThueSimCodeWrapper : IPhoneWrapper
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
        public ChoThueSimCodeWrapper(string apiKey)
        {
            this.choThueSimCodeApi = new ChoThueSimCodeApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="choThueSimCodeApi"></param>
        public ChoThueSimCodeWrapper(ChoThueSimCodeApi choThueSimCodeApi)
        {
            this.choThueSimCodeApi = choThueSimCodeApi ?? throw new ArgumentNullException(nameof(choThueSimCodeApi));
        }
        /// <summary>
        /// 
        /// </summary>
        ~ChoThueSimCodeWrapper()
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
        public async Task<IPhoneWrapperSession> RentPhone(CancellationToken cancellationToken = default)
        {
            if (ChoThueSimAppInfo == null) throw new InvalidOperationException($"{nameof(ChoThueSimAppInfo)} is null");
            var phone = await choThueSimCodeApi.GetPhoneNumber(ChoThueSimAppInfo, ChoThueSimCarrier, cancellationToken).ConfigureAwait(false);
            return new ChoThueSimCodeWrapperSession(choThueSimCodeApi, phone);
        }
    }


    internal class ChoThueSimCodeWrapperSession : IPhoneWrapperSession
    {
        readonly ChoThueSimCodeApi choThueSimCodeApi;
        readonly ChoThueSimBaseResult<ChoThueSimResponseCodeGetPhoneNumber, ChoThueSimPhoneNumberResult> phone;
        internal ChoThueSimCodeWrapperSession(
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

        public async Task<IEnumerable<IPhoneWrapperSms>> GetSms(CancellationToken cancellationToken = default)
        {
            var message = await choThueSimCodeApi.GetMessage(phone.Result, cancellationToken).ConfigureAwait(false);
            return new ChoThueSimCodeWrapperSms[] { new ChoThueSimCodeWrapperSms(message) };
        }
    }

    internal class ChoThueSimCodeWrapperSms : IPhoneWrapperSms
    {
        readonly ChoThueSimBaseResult<ChoThueSimResponseCodeMessage, ChoThueSimMessageResult> sms;
        internal ChoThueSimCodeWrapperSms(
            ChoThueSimBaseResult<ChoThueSimResponseCodeMessage, ChoThueSimMessageResult> sms)
        {
            this.sms = sms;
        }
        public string Text => sms?.Result?.SMS;

        public string Code => sms?.Result?.Code;
    }
}
