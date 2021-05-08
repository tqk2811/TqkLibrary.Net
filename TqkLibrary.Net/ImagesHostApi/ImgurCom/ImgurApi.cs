using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TqkLibrary.Net.ImagesHostApi.ImgurCom
{
  public class ImgurApi : BaseApi
  {
    private const string EndPoint = "https://api.imgur.com/3";

    public ImgurApi(string ApiKey) : base(ApiKey)
    {
    }

    public Task<ImgurResponse<ImgurImage>> UploadImage(byte[] bitmap)
    {
      using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{EndPoint}/Upload");
      //httpRequestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      //httpRequestMessage.Headers.Add("Authorization", $"Client-ID {ApiKey}");

      using MultipartFormDataContent requestContent = new MultipartFormDataContent();
      using ByteArrayContent imageContent_instructions = new ByteArrayContent(bitmap);
      imageContent_instructions.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
      requestContent.Add(imageContent_instructions, "image");
      requestContent.Add(new StringContent("file"), "type");
      httpRequestMessage.Content = requestContent;

      return RequestPost<ImgurResponse<ImgurImage>>(httpRequestMessage);
    }
  }
}