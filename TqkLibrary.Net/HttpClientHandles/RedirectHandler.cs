using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.HttpClientHandles
{
    /// <summary>
    /// 
    /// </summary>
    public class RedirectHandler : DelegatingHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public RedirectHandler(HttpMessageHandler innerHandler) : base(innerHandler ?? throw new ArgumentNullException(nameof(innerHandler)))
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public int MaxRedirectCount { get; set; } = 50;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var _request = request;
            var response = await base.SendAsync(_request, cancellationToken);
            int redirectCount = 0;
            while (true)
            {
                if (response.Headers.Location is null)
                    return response;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Moved://301
                    case HttpStatusCode.Redirect://302
                        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, response.Headers.Location);
                        foreach (var pair in request.Headers) httpRequestMessage.Headers.Add(pair.Key, pair.Value);
                        break;

                    case HttpStatusCode.RedirectMethod://303
                    case HttpStatusCode.RedirectKeepVerb://307
#if NET5_0_OR_GREATER
                    case HttpStatusCode.PermanentRedirect://308
#else
                    case (HttpStatusCode)308://308
#endif

                        break;

                    default:
                        return response;
                }
                redirectCount++;
                if (redirectCount >= MaxRedirectCount)
                {
                    return response;
                }
            }
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = base.Send(request, cancellationToken);


            return response;
        }
#endif
    }
}
