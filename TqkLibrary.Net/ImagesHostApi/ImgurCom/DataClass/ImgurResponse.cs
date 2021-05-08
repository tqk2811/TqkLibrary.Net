namespace TqkLibrary.Net.ImagesHostApi.ImgurCom
{
  public class ImgurResponse<T>
  {
    public bool success { get; set; }
    public int status { get; set; }
    public T data { get; set; }
  }
}