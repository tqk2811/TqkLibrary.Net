﻿using System;
using System.IO;
using TqkLibrary.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace TqkLibrary.Net.GoogleDocs
{
    public class SpreadsheetsHelper : BaseGoogleDocsHelper
    {
        public enum ExportFormat
        {
            xlsx,
            ods,
            //pdf,
            html,
            /// <summary>
            /// value split by comma (,) char
            /// </summary>
            csv,
            /// <summary>
            /// value split by tab char
            /// </summary>
            tsv
        }
        public Task<Stream> ExportAsync(ExportFormat format, string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            string f = format.GetAttribute<ExportTypeAttribute>()?.TypeName ?? format.ToString();

            return HandlerRedirect(new UrlBuilder("https://docs.google.com/spreadsheets/export")
                    .WithParam("format", f)
                    .WithParam("id", id),
                    cancellationToken);
        }
    }
}