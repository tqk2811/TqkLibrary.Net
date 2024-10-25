using System;
using System.IO;
using TqkLibrary.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace TqkLibrary.Net.GoogleDocs
{
    public class DocumentHelper : BaseApi
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
        public async Task<Stream> ExportAsync(ExportFormat format, string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            string f = format.GetAttribute< ExportTypeAttribute>()?.TypeName ?? format.ToString();

            HttpResponseMessage httpResponseMessage = await Build()
                .WithUrlGet(new UrlBuilder("https://docs.google.com/document/export")
                    .WithParam("format", f)
                    .WithParam("id", id)
                    )
                .WithHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7")
                .WithHeader("Referer", $"https://docs.google.com/document/d/{id}/edit")
                .ExecuteAsync(cancellationToken);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                return new HttpResponseStreamWrapper(httpResponseMessage, stream);
            }
            else
            {
                string? contentMediaType = httpResponseMessage.Content.Headers.ContentType?.MediaType;
                if (!string.IsNullOrWhiteSpace(contentMediaType))
                {
                    string[] cases = new string[] { "json", "text", "xml" };
                    if (cases.Any(x => contentMediaType!.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        string content = await httpResponseMessage.Content.ReadAsStringAsync();
                        throw new ApiException<string>($"Failed to export document '{id}'")
                        {
                            StatusCode = httpResponseMessage.StatusCode,
                            Body = content,
                        };
                    }
                }

                throw new ApiException($"Failed to export document '{id}'")
                {
                    StatusCode = httpResponseMessage.StatusCode,
                };
            }
        }
    }
}
