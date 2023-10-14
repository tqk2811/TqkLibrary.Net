using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TqkLibrary.Net.CloudStorage.OneDrive
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OneDriveApiNonLogin : IDisposable
    {
        readonly HttpClient _httpClient;

        /// <summary>
        /// 
        /// </summary>
        public OneDriveApiNonLogin() : this(
            new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = false,
            },
            true
        )
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public OneDriveApiNonLogin(HttpClientHandler httpClientHandler, bool disposeHandler) : this(new HttpClient(httpClientHandler, disposeHandler))
        {
            if (httpClientHandler.AllowAutoRedirect)
                throw new InvalidOperationException($"{nameof(httpClientHandler)}.{nameof(httpClientHandler.AllowAutoRedirect)} must be set to false");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected OneDriveApiNonLogin(HttpClient httpClient)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        /// <summary>
        /// 
        /// </summary>
        ~OneDriveApiNonLogin()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        void Dispose(bool isDisposing)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<OneDriveLinkInfo> DecodeShortLinkAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            if (!uri.Host.Equals("1drv.ms"))
                throw new InvalidOperationException($"Short link host must be '1drv.ms', current is '{uri.Host}'");

            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            using HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (httpResponseMessage.Headers.Location is null)
                return null;

            var query = HttpUtility.ParseQueryString(httpResponseMessage.Headers.Location.Query);
            string[] paths = uri.AbsolutePath.TrimStart('/').Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return new OneDriveLinkInfo()
            {
                ResourceId = query["resid"],
                AuthKey = query["authkey"],
                E = query["e"],
                Cid = query["cid"],
                ItHint = query["ithint"],
                Type = paths.FirstOrDefault(),
            };
        }


        const string EndPoint = "https://api.onedrive.com/v1.0/drives/";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<DriveItem> GetMetadataAsync(OneDriveLinkInfo oneDriveLinkInfo, CancellationToken cancellationToken = default)
            => GetMetadataAsync(oneDriveLinkInfo?.ResourceId, oneDriveLinkInfo?.AuthKey, cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DriveItem> GetMetadataAsync(string itemId, string authKey, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentNullException(nameof(itemId));
            if (string.IsNullOrWhiteSpace(authKey))
                throw new ArgumentNullException(nameof(authKey));
            if (!itemId.Contains("!") || itemId.Count(x => x == '!') != 1)
                throw new InvalidOperationException($"invalid {nameof(itemId)}, must be contain only one '!'");
            string[] itemIdSplit = itemId.Split('!');

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["select"] = "createdBy,createdDateTime,description,driveId,file,fileSystemInfo,id,@content.downloadUrl,image,lastModifiedDateTime,location,name,parentReference,photo,size,shared,tags,video,viewpoint";
            query["expand"] = "tags";
            query["authkey"] = authKey;

            var url = new UrlBuilder(EndPoint, itemIdSplit[0], "items", itemId)
                .WithParams(query);

            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            httpRequestMessage.Headers.Add("Origin", "https://onedrive.live.com");
            httpRequestMessage.Headers.Referrer = new Uri("https://onedrive.live.com");

            using HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);

            string text = await httpResponseMessage.Content.ReadAsStringAsync();
            httpResponseMessage.EnsureSuccessStatusCode();
            JsonParseNode jsonParseNode = new JsonParseNode(JsonDocument.Parse(text).RootElement);
            return jsonParseNode.GetObjectValue(DriveItem.CreateFromDiscriminatorValue);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<Stream> DownloadFileAsync(OneDriveLinkInfo oneDriveLinkInfo, CancellationToken cancellationToken = default)
            => DownloadFileAsync(oneDriveLinkInfo?.ResourceId, oneDriveLinkInfo?.AuthKey, cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> DownloadFileAsync(string itemId, string authKey, CancellationToken cancellationToken = default)
        {
            var driverItem = await GetMetadataAsync(itemId, authKey, cancellationToken);
            return await DownloadFileAsync(driverItem, cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> DownloadFileAsync(DriveItem driveItem, CancellationToken cancellationToken = default)
        {
            if (driveItem.AdditionalData.TryGetValue("@content.downloadUrl", out object val) && val is string url)
            {
                using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add("Origin", "https://onedrive.live.com");
                httpRequestMessage.Headers.Referrer = new Uri("https://onedrive.live.com");

                HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
                return new HttpResponseStreamWrapper(httpResponseMessage, await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStreamAsync());
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<DriveItemCollectionResponse> ListChildItems(OneDriveLinkInfo oneDriveLinkInfo, CancellationToken cancellationToken = default)
            => ListChildItems(oneDriveLinkInfo?.ResourceId, oneDriveLinkInfo?.AuthKey, cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<DriveItemCollectionResponse> ListChildItems(string itemId, string authKey, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentNullException(nameof(itemId));
            if (string.IsNullOrWhiteSpace(authKey))
                throw new ArgumentNullException(nameof(authKey));
            if (!itemId.Contains("!") || itemId.Count(x => x == '!') != 1)
                throw new InvalidOperationException($"invalid {nameof(itemId)}, must be contain only one '!'");
            string[] itemIdSplit = itemId.Split('!');

            var query = HttpUtility.ParseQueryString(string.Empty);
            //query["top"] = "100";
            //query["expand"] = "thumbnails,lenses,tags";
            query["select"] = "*,ocr,webDavUrl,sharepointIds,isRestricted,commentSettings,specialFolder,containingDrivePolicyScenarioViewpoint";
            query["authkey"] = authKey;
            query["ump"] = "1";

            var url = new UrlBuilder(EndPoint, itemIdSplit[0].ToLower(), "items", itemId, "children")
                .WithParams(query);

            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            httpRequestMessage.Headers.Add("Origin", "https://onedrive.live.com");
            httpRequestMessage.Headers.Referrer = new Uri("https://onedrive.live.com");

            using HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);

            string text = await httpResponseMessage.Content.ReadAsStringAsync();
            httpResponseMessage.EnsureSuccessStatusCode();
            JsonParseNode jsonParseNode = new JsonParseNode(JsonDocument.Parse(text).RootElement);
            return jsonParseNode.GetObjectValue(DriveItemCollectionResponse.CreateFromDiscriminatorValue);
        }
    }
}