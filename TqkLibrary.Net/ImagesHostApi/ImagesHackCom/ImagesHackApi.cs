using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TqkLibrary.Net.ImagesHostApi.ImagesHackCom
{
  //https://docs.google.com/document/d/16M3qaw27vgwuwXqExo0aIC0nni42OOuWu_OGvpYl7dE/pub#h.jcrh03smytne
  public class ImagesHackApi : BaseApi
  {
    private const string EndPoint = "https://api.imageshack.com/v2/images";

    public ImagesHackApi(string ApiKey) : base(ApiKey)
    {
    }

    public Task<ImagesHackResponse<ImagesHackUploadResult>> UploadImage(Bitmap bitmap)
    {
      byte[] buffer_bitmap = null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        bitmap.Save(memoryStream, ImageFormat.Jpeg);//hoac png
        memoryStream.Position = 0;
        buffer_bitmap = new byte[memoryStream.Length];
        memoryStream.Read(buffer_bitmap, 0, (int)memoryStream.Length);
      }
      return UploadImage(buffer_bitmap);
    }

    public Task<ImagesHackResponse<ImagesHackUploadResult>> UploadImage(byte[] bitmap)
    {
      MultipartFormDataContent requestContent = new MultipartFormDataContent();
      ByteArrayContent imageContent_instructions = new ByteArrayContent(bitmap);
      imageContent_instructions.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
      requestContent.Add(imageContent_instructions, "file", "file.jpeg");
      return RequestPost<ImagesHackResponse<ImagesHackUploadResult>>(new Uri(EndPoint + $"?api_key={ApiKey}"), requestContent);
    }
  }
}