using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.HttpClientHandles;
using System.Net.Http.Headers;

namespace TqkLibrary.Net.GoogleDocs
{
    public abstract class BaseGoogleDocsHelper : BaseApi
    {
        public BaseGoogleDocsHelper() : this(new CookieHandler(new HttpClientHandler() { AllowAutoRedirect = false, }))
        {
            this.httpClient.DefaultRequestHeaders.Referrer = new Uri("https://docs.google.com");
            this.httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }

        readonly CookieHandler cookieHandler;
        private BaseGoogleDocsHelper(CookieHandler cookieHandler) : base(".", cookieHandler)
        {
            this.cookieHandler = cookieHandler;
        }


        protected async Task<Stream> HandlerRedirect(UrlBuilder urlBuilder,CancellationToken cancellationToken = default)
        {
            using HttpResponseMessage res = await Build()
                .WithUrlGet(urlBuilder)
                .ExecuteAsync(cancellationToken);
            if (res.Headers.Location is null)
            {
                res.EnsureSuccessStatusCode();
                throw new InvalidOperationException($"Location was null and StatusCode: {res.StatusCode}");
            }

            HttpResponseMessage res2 = await Build()
                .WithUrlGet(res.Headers.Location!)
                .ExecuteAsync(cancellationToken);

            try
            {
                var stream = await res2.EnsureSuccessStatusCode().Content.ReadAsStreamAsync();
                return new HttpResponseStreamWrapper(res2, stream);
            }
            catch
            {
                res2.Dispose();
                throw;
            }
        }
    }
}
