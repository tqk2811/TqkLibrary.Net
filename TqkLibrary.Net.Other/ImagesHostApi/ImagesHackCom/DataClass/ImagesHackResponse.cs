namespace TqkLibrary.Net.ImagesHostApi
{
  public class ImagesHackResponse<T>
  {
    public bool? success { get; set; }
    public int? process_time { get; set; }
    public T result { get; set; }
  }
}