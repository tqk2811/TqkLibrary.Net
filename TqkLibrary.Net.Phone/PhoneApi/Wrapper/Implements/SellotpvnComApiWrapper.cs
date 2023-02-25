using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Phone.PhoneApi.Wrapper.Implements
{
    public class SellotpvnComApiWrapper : IPhoneWrapper
    {
        readonly SellotpvnComApi sellotpvnComApi;
        public SellotpvnComApiWrapper(SellotpvnComApi.ApiEndPoint apiEndPoint, string token) : this(new SellotpvnComApi(apiEndPoint, token))
        {

        }
        public SellotpvnComApiWrapper(SellotpvnComApi sellotpvnComApi)
        {
            this.sellotpvnComApi = sellotpvnComApi ?? throw new ArgumentNullException(nameof(sellotpvnComApi));
        }

        public SellotpvnComApi.Serivce Service { get; set; }
        public SellotpvnComApi.NetworkProvider? NetworkProvider { get; set; }

        public void Dispose()
        {
            sellotpvnComApi.Dispose();
        }

        public async Task<IPhoneWrapperSession> RentPhone(CancellationToken cancellationToken = default)
        {
            if (Service is null) throw new InvalidOperationException($"{nameof(SellotpvnComApiWrapper)}.{nameof(Service)} musbe set");
            var response = await sellotpvnComApi.Order(Service, NetworkProvider, cancellationToken).ConfigureAwait(false);
            return new PhoneWrapperSession(sellotpvnComApi, response);
        }

        class PhoneWrapperSession : IPhoneWrapperSession
        {
            readonly SellotpvnComApi sellotpvnComApi;
            readonly SellotpvnComApi.Response response;
            public PhoneWrapperSession(SellotpvnComApi sellotpvnComApi, SellotpvnComApi.Response response)
            {
                this.sellotpvnComApi = sellotpvnComApi ?? throw new ArgumentNullException(nameof(sellotpvnComApi));
                this.response = response ?? throw new ArgumentNullException(nameof(response));
            }
            public bool IsSuccess => response.Status == SellotpvnComApi.Status.Pending;

            public string PhoneNumber => response.PhoneNumber;

            public string Message => response.otp;

            public Task CancelWaitSms(CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<IPhoneWrapperSmsResult<IPhoneWrapperSms>> GetSms(CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }
    }
}
