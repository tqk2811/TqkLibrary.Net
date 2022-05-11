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
    public class FiveSimNetManaged : IPhoneApi
    {
        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; } = "any";
        /// <summary>
        /// 
        /// </summary>
        public string Operator { get; set; } = "any";
        /// <summary>
        /// 
        /// </summary>
        public string Product { get; set; }


        readonly FiveSimNetApi fiveSimNetApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public FiveSimNetManaged(string apiKey)
        {
            this.fiveSimNetApi = new FiveSimNetApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fiveSimNetApi"></param>
        public FiveSimNetManaged(FiveSimNetApi fiveSimNetApi)
        {
            this.fiveSimNetApi = fiveSimNetApi;
        }
        /// <summary>
        /// 
        /// </summary>
        ~FiveSimNetManaged()
        {
            fiveSimNetApi.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            fiveSimNetApi.Dispose();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPhoneSession> RentPhone(CancellationToken cancellationToken = default)
        {
            var res = await fiveSimNetApi.BuyActivationNumber(Country, Operator, Product, cancellationToken).ConfigureAwait(false);
            return new FiveSimNetManagedSession(fiveSimNetApi, res);
        }
    }

    internal class FiveSimNetManagedSession : IPhoneSession
    {
        readonly FiveSimNetApi fiveSimNetApi;
        readonly FiveSimNetNumber fiveSimNetNumber;
        internal FiveSimNetManagedSession(FiveSimNetApi fiveSimNetApi, FiveSimNetNumber fiveSimNetNumber)
        {
            this.fiveSimNetApi = fiveSimNetApi;
            this.fiveSimNetNumber = fiveSimNetNumber;
        }
        public string PhoneNumber => fiveSimNetNumber?.Phone;

        public Task CancelWaitSms(CancellationToken cancellationToken = default)
        {
            return fiveSimNetApi.CancelOrder(fiveSimNetNumber, cancellationToken);
        }

        public async Task<IEnumerable<IPhoneSms>> GetSms(CancellationToken cancellationToken = default)
        {
            var res = await fiveSimNetApi.CheckOrder(fiveSimNetNumber, cancellationToken).ConfigureAwait(false);
            return res.Sms.Select(x => new FiveSimNetManagedSms(x));
        }
    }

    internal class FiveSimNetManagedSms : IPhoneSms
    {
        readonly FiveSimNetSms fiveSimNetSms;
        internal FiveSimNetManagedSms(FiveSimNetSms fiveSimNetSms)
        {
            this.fiveSimNetSms = fiveSimNetSms;
        }
        public string Text => fiveSimNetSms?.Text;

        public string Code => fiveSimNetSms?.Code;
    }
}
