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
            pdf,
            txt,
            [ExportType("zip")] ziphtml,
            epub,
            md
        }
        public async Task<Stream> ExportAsync(ExportFormat format, string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            string f = format.GetType().GetCustomAttribute<ExportTypeAttribute>()?.TypeName ?? format.ToString();

            HttpResponseMessage httpResponseMessage = await Build()
                .WithUrlGet(new UrlBuilder("https://docs.google.com/document/export")
                    .WithParam("format", f)
                    .WithParam("id", id)
                    .WithParam("includes_info_params", "true")
                    .WithParam("cros_files", "false")
                    .WithParam("tab", "t.0")
                    .WithParam("inspectorResult", "{\"pc\":3,\"lplc\":7}")
                    )
                .WithHeader("Accept", "*/*")
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
