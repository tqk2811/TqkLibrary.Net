using Newtonsoft.Json;

namespace TqkLibrary.Net.ImagesHostApi.ImagesHackCom
{
  public class ImageHackAlbum
  {
    public string id { get; set; }
    public string title { get; set; }

    [JsonProperty("public")]
    public bool? IsPublic { get; set; }
  }
}