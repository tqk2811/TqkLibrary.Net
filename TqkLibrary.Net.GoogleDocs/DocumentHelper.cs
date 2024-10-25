using System;
using System.IO;
using TqkLibrary.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using TqkLibrary.Net.HttpClientHandles;
using System.Net.Http.Headers;

namespace TqkLibrary.Net.GoogleDocs
{
    public class DocumentHelper : BaseGoogleDocsHelper
    {
        public enum ExportFormat
        {
            docx,
            odt,
            rtf,
            //pdf,
            txt,
            [ExportType("zip")] ziphtml,
            epub,
            md
        }
        public Task<Stream> ExportAsync(ExportFormat format, string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            string f = format.GetAttribute<ExportTypeAttribute>()?.TypeName ?? format.ToString();

            return HandlerRedirect(new UrlBuilder("https://docs.google.com/document/export")
                    .WithParam("format", f)
                    .WithParam("id", id),
                    cancellationToken);
        }
    }
}
