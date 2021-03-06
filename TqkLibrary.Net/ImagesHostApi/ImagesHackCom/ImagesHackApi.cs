using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.ImagesHostApi
{
    //https://docs.google.com/document/d/16M3qaw27vgwuwXqExo0aIC0nni42OOuWu_OGvpYl7dE/pub#h.jcrh03smytne
    /// <summary>
    /// 
    /// </summary>
    public class ImagesHackApi : BaseApi
    {
        private const string EndPoint = "https://api.imageshack.com/v2/images";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApiKey"></param>
        public ImagesHackApi(string ApiKey) : base(ApiKey)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<ImagesHackResponse<ImagesHackUploadResult>> UploadImage(Bitmap bitmap, CancellationToken cancellationToken = default)
          => UploadImage(bitmap.BitmapToBuffer(), cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<ImagesHackResponse<ImagesHackUploadResult>> UploadImage(byte[] bitmap, CancellationToken cancellationToken = default)
        {
            MultipartFormDataContent requestContent = new MultipartFormDataContent();
            ByteArrayContent imageContent_instructions = new ByteArrayContent(bitmap);
            imageContent_instructions.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent_instructions, "file", "file.jpeg");

            return Build()
            .WithUrlPost(new UriBuilder(EndPoint).WithParam("api_key", ApiKey), requestContent)
            .ExecuteAsync<ImagesHackResponse<ImagesHackUploadResult>>(cancellationToken);
        }
    }
}