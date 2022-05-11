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
    public class TwoLineIoWrapper : IPhoneWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public TwoLineIoService Service { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TwoLineIoNetWorkId? NetWorkId { get; set; }





        readonly TwoLineIoApi twoLineIoApi;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="twoLineIoApi"></param>
        public TwoLineIoWrapper(TwoLineIoApi twoLineIoApi)
        {
            this.twoLineIoApi = twoLineIoApi;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        public TwoLineIoWrapper(string apiKey)
        {
            this.twoLineIoApi = new TwoLineIoApi(apiKey);
        }
        /// <summary>
        /// 
        /// </summary>
        ~TwoLineIoWrapper()
        {
            twoLineIoApi.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            twoLineIoApi.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IPhoneWrapperSession> RentPhone(CancellationToken cancellationToken = default)
        {
            if (Service == null) throw new InvalidOperationException($"set {nameof(Service)} first");
            var res = await twoLineIoApi.PurchaseOTP(Service, NetWorkId, null, cancellationToken).ConfigureAwait(false);
            var check = await twoLineIoApi.CheckOrder(res, cancellationToken).ConfigureAwait(false);
            return new TwoLineIoWrapperSession(twoLineIoApi, res, check.Data);
        }
    }
    internal class TwoLineIoWrapperSession : IPhoneWrapperSession
    {
        readonly TwoLineIoApi twoLineIoApi;
        readonly TwoLineIoPurchaseOtpResponse twoLineIoPurchaseOtpResponse;
        readonly TwoLineIoOrderData twoLineIoOrderData;
        internal TwoLineIoWrapperSession(
            TwoLineIoApi twoLineIoApi, 
            TwoLineIoPurchaseOtpResponse twoLineIoPurchaseOtpResponse, 
            TwoLineIoOrderData twoLineIoOrderData)
        {
            this.twoLineIoApi = twoLineIoApi;
            this.twoLineIoPurchaseOtpResponse = twoLineIoPurchaseOtpResponse;
            this.twoLineIoOrderData = twoLineIoOrderData;
        }


        public string PhoneNumber => twoLineIoOrderData?.Phone;

        public Task CancelWaitSms(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<IPhoneWrapperSms>> GetSms(CancellationToken cancellationToken = default)
        {
            var order = await twoLineIoApi.CheckOrder(twoLineIoPurchaseOtpResponse, cancellationToken).ConfigureAwait(false);
            return new TwoLineIoWrapperSms[] { new TwoLineIoWrapperSms(order.Data) };
        }
    }

    internal class TwoLineIoWrapperSms : IPhoneWrapperSms
    {
        readonly TwoLineIoOrderData twoLineIoOrderData;
        internal TwoLineIoWrapperSms(TwoLineIoOrderData twoLineIoOrderData)
        {
            this.twoLineIoOrderData = twoLineIoOrderData;
        }

        public string Text => twoLineIoOrderData?.Message;

        public string Code => twoLineIoOrderData?.Code;
    }
}
