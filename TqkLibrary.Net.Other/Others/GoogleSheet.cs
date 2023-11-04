using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public GoogleSheet()
        {
            this.httpClient.DefaultRequestHeaders.Referrer = new Uri("https://docs.google.com");
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
            var res = await Build()
                .WithUrlGet(new UrlBuilder("https://docs.google.com/spreadsheets/d", fileId, "export")
                    .WithParam("format", format)
                    .WithParam("id", fileId))
                .ExecuteAsync(cancellationToken);
            try
            {
                var stream = await res.EnsureSuccessStatusCode().Content.ReadAsStreamAsync();
                return new HttpResponseStreamWrapper(res, stream);
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
