namespace TqkLibrary.Net.ImagesHostApi
{
  public class ImgurResponse<T>
  {
    public bool success { get; set; }
    public int status { get; set; }
    public T data { get; set; }
  }
}