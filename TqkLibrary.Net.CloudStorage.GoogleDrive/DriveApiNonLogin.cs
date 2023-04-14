using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Newtonsoft.Json;

namespace TqkLibrary.Net.CloudStorage.GoogleDrive
{
    public class DriveApiNonLogin : IDisposable
    {
        public string ApiKey { get; set; } = "AIzaSyC1qbk75NzWBvSaDh6KnsjjA9pIrP4lYIE";
        readonly HttpClient httpClient;
        public DriveApiNonLogin() : this(
            new HttpClient(
                new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    AllowAutoRedirect = true,
                }
                ))
        {

        }
        public DriveApiNonLogin(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        ~DriveApiNonLogin()
        {
            httpClient.Dispose();
        }
        public void Dispose()
        {
            httpClient.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<FileList> ListPublicFolderAsync(DriveFileListOption option, CancellationToken cancellationToken = default)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));
            string url = option.NextLink;

            if (string.IsNullOrEmpty(url))
            {
                if (string.IsNullOrWhiteSpace(option.Query)) throw new ArgumentNullException(option.Query.GetType().FullName);
                if (string.IsNullOrWhiteSpace(option.Fields)) throw new ArgumentNullException(option.Fields.GetType().FullName);
                if (string.IsNullOrWhiteSpace(option.OrderBy)) throw new ArgumentNullException(option.OrderBy.GetType().FullName);

                var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
                queryBuilder.Add("openDrive", "false");
                queryBuilder.Add("reason", "102");
                queryBuilder.Add("syncType", "0");
                queryBuilder.Add("errorRecovery", "false");
                queryBuilder.Add("appDataFilter", "NO_APP_DATA");
                queryBuilder.Add("spaces", "drive");
                queryBuilder.Add("maxResults", option.MaxResults.ToString());
                queryBuilder.Add("supportsTeamDrives", option.SupportsTeamDrives.ToString().ToLower());
                queryBuilder.Add("includeTeamDriveItems", option.IncludeTeamDriveItems.ToString().ToLower());
                queryBuilder.Add("corpora", "default");
                queryBuilder.Add("retryCount", "0");
                queryBuilder.Add("key", ApiKey);
                queryBuilder.Add("q", option.Query);
                queryBuilder.Add("fields", option.Fields);
                queryBuilder.Add("orderBy", option.OrderBy);
                if (!string.IsNullOrEmpty(option.PageToken)) queryBuilder.Add("pageToken", option.PageToken);

                url = $"https://clients6.google.com/drive/v2beta/files?{queryBuilder.ToString().Replace("+", "%20")}";
            }

            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequestMessage.Headers.Referrer = new Uri("https://drive.google.com/");
            httpRequestMessage.Headers.Add("Accept", "application/json");
            httpRequestMessage.Headers.Add("x-goog-drive-resource-keys", $"{option.FolderId}/{option.Resourcekey}");
            using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
            string json_text = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<FileList>(json_text);
        }

        public async Task<Google.Apis.Drive.v2.Data.File> GetMetadataAsync(string fileId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(fileId)) throw new ArgumentNullException(fileId);

            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);
            queryBuilder.Add("supportsTeamDrives", "true");
            queryBuilder.Add("includeBadgedLabels", "true");
            queryBuilder.Add("enforceSingleParent", "true");
            queryBuilder.Add("key", ApiKey);
            queryBuilder.Add("fields", "*");

            string url = $"https://content.googleapis.com/drive/v2beta/files/{fileId}?{queryBuilder.ToString()}";
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequestMessage.Headers.Referrer = new Uri("https://drive.google.com/");
            httpRequestMessage.Headers.Add("Accept", "application/json");
            using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);
            string json_text = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Google.Apis.Drive.v2.Data.File>(json_text);
        }


        static readonly Regex regex_confirmDriveDownload = new Regex("(?<=action=\")https:\\/\\/drive.google.com\\/uc?.*?(?=\")");
        public async Task<Stream> DownloadFileAsync(string fileId, CancellationToken cancellationToken = default)
        {
            string url = $"https://drive.google.com/uc?export=download&id={fileId}";
            HttpMethod method = HttpMethod.Get;
            while (true)
            {
                using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, url);
                //httpRequestMessage.Headers.Referrer = new Uri("https://drive.google.com/");
                //httpRequestMessage.Headers.Add("Origin", "https://drive.google.com/");
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                httpResponseMessage.EnsureSuccessStatusCode();
                if (httpResponseMessage.Content.Headers.ContentType.MediaType.Contains("text/html"))
                {
                    string content = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                    Match match = regex_confirmDriveDownload.Match(content);
                    if (match.Success)//file large, can't scan virus, need confirm
                    {
                        url = HttpUtility.HtmlDecode(match.Value);
                        method = HttpMethod.Post;
                        continue;
                    }
                }
                else// if (httpResponseMessage.Content.Headers.ContentType.MediaType.Contains("application"))
                {
                    Stream stream = await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStreamAsync();
                    return new StreamWrapper(httpResponseMessage, stream);
                }
                //throw new InvalidDataException($"Can't find download link for fileId: {fileId}, {url}");
            }
        }
    }
}