using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Facebook
{
  public class FacebookApi
  {
    private const string ApiEndPoint = "https://graph.facebook.com";
    private readonly string Api;

    public FacebookApi(string version = "v8.0")
    {
      Api = ApiEndPoint + "//" + version;
    }

    //oauth
    public async Task<FacebookToken> GetAccessToken(string code, string AppId, string AppSecret, string redirect_uri)
    {
      using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
        Api + $"/oauth/access_token?client_id={AppId}&redirect_uri={redirect_uri}&client_secret={AppSecret}&code={code}"))
      {
        using (HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead))
        {
          return new FacebookToken(JsonConvert.DeserializeObject<FacebookToken_>(await httpResponseMessage.Content.ReadAsStringAsync()));
        }
      }
    }

    public async Task<FacebookUser> GetCurrentUser(string access_token)
    {
      using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, Api + $"/me?access_token={access_token}"))
      {
        using (HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead))
        {
          return JsonConvert.DeserializeObject<FacebookUser>(await httpResponseMessage.Content.ReadAsStringAsync());
        }
      }
    }

    public async Task<DataPages> ListAllPages(string access_token)
    {
      using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, Api + $"/me/accounts?access_token={access_token}"))
      {
        using (HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead))
        {
          return JsonConvert.DeserializeObject<DataPages>(await httpResponseMessage.Content.ReadAsStringAsync());
        }
      }
    }

    public async Task<string> PagePostContent(string access_token, string content, string link = null, bool published = true, DateTime? ScheduleTime = null)
    {
      var dict = new Dictionary<string, string>();
      dict.Add("message", content);
      dict.Add("access_token", access_token);
      dict.Add("published", published.ToString());
      if (!published && ScheduleTime != null)
      {
        dict.Add("scheduled_publish_time", new DateTimeOffset(ScheduleTime.Value).ToUnixTimeSeconds().ToString());//4.6.2
      }
      if (!string.IsNullOrEmpty(link)) dict.Add("link", link);

      using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, Api + $"/me/feed"))
      {
        httpRequestMessage.Content = new FormUrlEncodedContent(dict);
        using (HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead))
        {
          return await httpResponseMessage.Content.ReadAsStringAsync();
        }
      }
    }

    public async Task<string> UploadingPhoto(string access_token, string photo_url, bool published)
    {
      var dict = new Dictionary<string, string>();
      dict.Add("url", photo_url);
      dict.Add("access_token", access_token);
      dict.Add("published", published.ToString());
      if (!published) dict.Add("temporary", true.ToString());
      using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, Api + $"/me/photos"))
      {
        httpRequestMessage.Content = new FormUrlEncodedContent(dict);
        using (HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead))
        {
          return await httpResponseMessage.Content.ReadAsStringAsync();
        }
      }
    }

    public async Task<string> UploadingPhoto(string access_token, byte[] image, bool published)
    {
      using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, Api + $"/me/photos"))
      {
        MultipartFormDataContent form = new MultipartFormDataContent();
        form.Add(new StringContent(access_token), "access_token");
        form.Add(new StringContent(published.ToString()), "published");
        if (!published) form.Add(new StringContent(true.ToString()), "temporary");
        form.Add(new ByteArrayContent(image), "image", "image.jpg");

        httpRequestMessage.Content = form;
        using (HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead))
        {
          return await httpResponseMessage.Content.ReadAsStringAsync();
        }
      }
    }

    public async Task<string> PublishingMultiPhoto(string access_token, string message, IEnumerable<string> imgsId, bool published = true, DateTime? time = null)
    {
      var dict = new Dictionary<string, string>();
      dict.Add("message", message);
      dict.Add("access_token", access_token);
      dict.Add("published", published ? "1" : "0");
      if (!published && time != null)
      {
        dict.Add("scheduled_publish_time", new DateTimeOffset(time.Value).ToUnixTimeSeconds().ToString());
        dict.Add("unpublished_content_type", "SCHEDULED");
      }

      int i = 0;
      foreach (var id in imgsId) dict.Add("attached_media[" + i++ + "]", "{\"media_fbid\":\"" + id + "\"}");

      using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, Api + $"/me/feed"))
      {
        httpRequestMessage.Content = new FormUrlEncodedContent(dict);
        using (HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead))
        {
          return await httpResponseMessage.Content.ReadAsStringAsync();
        }
      }
    }

    public async Task<byte[]> PictureByte(string access_token, int width = 9999, int height = 9999, string userId = null)
    {
      string url = Api + $"/{(string.IsNullOrEmpty(userId) ? "me" : userId)}/picture?access_token={access_token}&width={width}&{height}=9999";//&type=large square, small, normal, large
      using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url))
      {
        using (HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead))
        {
          return await httpResponseMessage.Content.ReadAsByteArrayAsync();
        }
      }
    }

    public async Task<Bitmap> PictureBitMap(string access_token, int width = 9999, int height = 9999, string userId = null)
    {
      byte[] buffer = await PictureByte(access_token, width, height, userId);
      MemoryStream ms = new MemoryStream();//bitmap auto dispose stream when bitmap dispose
      ms.Write(buffer, 0, buffer.Length);
      ms.Seek(0, SeekOrigin.Begin);
      return (Bitmap)Bitmap.FromStream(ms);
    }

    public async Task<string> UserInfo(string access_token, string fields = "birthday,name", string userId = null)
    {
      string url = Api + $"/{(string.IsNullOrEmpty(userId) ? "me" : userId)}?access_token={access_token}&fields={fields}";
      using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url))
      {
        using (HttpResponseMessage httpResponseMessage = await NetExtensions.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead))
        {
          return await httpResponseMessage.Content.ReadAsStringAsync();
        }
      }
    }
  }
}