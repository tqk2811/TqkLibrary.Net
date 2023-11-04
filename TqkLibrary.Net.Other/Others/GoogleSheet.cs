using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TqkLibrary.Net.HttpClientHandles;

namespace TqkLibrary.Net.Other.Others
{
    /// <summary>
    /// 
    /// </summary>
    public class GoogleSheet : BaseApi
    {
        /// <summary>
        /// 
        /// </summary>
        public GoogleSheet() : this(new CookieHandler(new HttpClientHandler()
        {
            AllowAutoRedirect = false,
        }))
        {
            this.httpClient.DefaultRequestHeaders.Referrer = new Uri("https://docs.google.com");
        }
        readonly CookieHandler cookieHandler;
        private GoogleSheet(CookieHandler cookieHandler) : base(".", cookieHandler)
        {
            this.cookieHandler = cookieHandler;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="format"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Stream> ExportAsync(string fileId, ExportExcelType format = ExportExcelType.xlsx, CancellationToken cancellationToken = default)
        {
            //var res_html = await Build()
            //    .WithUrlGet(new UrlBuilder("https://docs.google.com/spreadsheets/d", fileId, "edit"))
            //    .ExecuteAsync(cancellationToken);

            var res = await Build()
                .WithUrlGet(new UrlBuilder("https://docs.google.com/spreadsheets/d", fileId, "export")
                    .WithParam("format", format)
                    .WithParam("id", fileId))
                .ExecuteAsync(cancellationToken);
            if (res.Headers.Location is null)
            {
                res.EnsureSuccessStatusCode();
            }
            var res2 = await Build()
                .WithUrlGet(res.Headers.Location)
                .ExecuteAsync(cancellationToken);
            try
            {
                var stream = await res2.EnsureSuccessStatusCode().Content.ReadAsStreamAsync();
                return new HttpResponseStreamWrapper(res2, stream);
            }
            catch
            {
                res.Dispose();
                throw;
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public enum ExportExcelType
        {
            xlsx,
            ods,
            csv,
            tsv,
            zip
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
