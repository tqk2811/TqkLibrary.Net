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
            //this.httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            //this.httpClient.DefaultRequestHeaders.Add("Accept-Language", "en");
            //this.httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            //this.httpClient.DefaultRequestHeaders.Add("Dnt", "1");

            //this.httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua", "\"Chromium\";v=\"118\", \"Google Chrome\";v=\"118\", \"Not=A?Brand\";v=\"99\"");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua-Arch", "\"x86\"");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua-Bitness", "\"64\"");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua-Full-Version-List", "\"Chromium\";v=\"118.0.5993.120\", \"Google Chrome\";v=\"118.0.5993.120\", \"Not=A?Brand\";v=\"99.0.0.0\"");
            //this.httpClient.DefaultRequestHeaders.Add("", "");
            //this.httpClient.DefaultRequestHeaders.Add("", "");
            //this.httpClient.DefaultRequestHeaders.Add("", "");
            //this.httpClient.DefaultRequestHeaders.Add("", "");
            //this.httpClient.DefaultRequestHeaders.Add("", "");
            //this.httpClient.DefaultRequestHeaders.Add("", "");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform-Version", "\"10.0.0\"");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua-Model", "\"\"");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "iframe");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            //this.httpClient.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");

            //this.httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            //this.httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/118.0.0.0 Safari/537.36");
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
