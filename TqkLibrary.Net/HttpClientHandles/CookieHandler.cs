using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.HttpClientHandles
{
    //https://gist.github.com/damianh/038195c1ab0c5013ad3883d7e3c59d99
    /// <summary>
    /// 
    /// </summary>
    public class CookieHandler : DelegatingHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public CookieContainer CookieContainer { get; }

        /// <summary>
        /// 
        /// </summary>
        public CookieHandler(HttpMessageHandler innerHandler) : base(innerHandler ?? throw new ArgumentNullException(nameof(innerHandler)))
        {
            this.CookieContainer = new CookieContainer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public CookieHandler(CookieContainer cookieContainer, HttpMessageHandler innerHandler) : base(innerHandler ?? throw new ArgumentNullException(nameof(innerHandler)))
        {
            this.CookieContainer = cookieContainer ?? throw new ArgumentNullException(nameof(cookieContainer));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            request.Headers.Add("Cookie", CookieContainer.GetCookieHeader(request.RequestUri));

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.Headers.TryGetValues("Set-Cookie", out var newCookies))
            {
                foreach (var item in SetCookieHeaderValue.ParseList(newCookies.ToList()))
                {
                    var uri = new Uri(request.RequestUri, item.Path.Value);
                    CookieContainer.Add(uri, new Cookie(item.Name.Value, item.Value.Value, item.Path.Value));
                }
            }

            return response;
        }
    }
}
