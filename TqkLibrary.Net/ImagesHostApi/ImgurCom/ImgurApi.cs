using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.ImagesHostApi
{
    /// <summary>
    /// 
    /// </summary>
    public class ImgurApi : BaseApi
    {
        private const string EndPoint = "https://api.imgur.com/3";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApiKey"></param>
        public ImgurApi(string ApiKey) : base(ApiKey)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<ImgurResponse<ImgurImage>> UploadImage(Bitmap bitmap, CancellationToken cancellationToken = default)
          => UploadImage(bitmap.BitmapToBuffer());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<ImgurResponse<ImgurImage>> UploadImage(byte[] bitmap, CancellationToken cancellationToken = default)
        {
            MultipartFormDataContent requestContent = new MultipartFormDataContent();
            ByteArrayContent imageContent_instructions = new ByteArrayContent(bitmap);
            imageContent_instructions.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent_instructions, "image");
            requestContent.Add(new StringContent("file"), "type");
            return Build()
                .WithUrlPost(new UriBuilder(EndPoint, "upload"), requestContent)
                .WithHeader("Authorization", $"Client-ID {ApiKey}")
                .ExecuteAsync<ImgurResponse<ImgurImage>>(cancellationToken);
        }
    }
}