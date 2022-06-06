using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public static class NetExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<MyIp> GetCurrentIpAdreess()
        {
            using HttpClient httpClient = new HttpClient(NetSingleton.HttpClientHandler, false);
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.myip.com");
            using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<MyIp>(await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBuffer(this Bitmap bitmap)
        {
            byte[] buffer_bitmap = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Jpeg);//hoac png
                memoryStream.Position = 0;
                buffer_bitmap = new byte[memoryStream.Length];
                memoryStream.Read(buffer_bitmap, 0, (int)memoryStream.Length);
            }
            return buffer_bitmap;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Join(this IEnumerable<string> enumerable, string separator)
        {
            if (string.IsNullOrWhiteSpace(separator)) throw new ArgumentNullException(nameof(separator));
            if (enumerable == null) return null;
            return string.Join(separator, enumerable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetDomain(this Uri uri) => $"{uri.Scheme}://{uri.Authority}";
    }
}