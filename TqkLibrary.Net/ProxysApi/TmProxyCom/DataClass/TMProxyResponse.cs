namespace TqkLibrary.Net.ProxysApi.TmProxyCom
{
  public class TMProxyResponse<T>
  {
    public int code { get; set; }
    public string message { get; set; }
    public T data { get; set; }
  }
}
