namespace TqkLibrary.Net.PhoneNumberApi.ChoThueSimCodeCom
{
  public class BaseResult<T1, T2>
  {
    public T1 ResponseCode { get; set; }
    public string Msg { get; set; }
    public T2 Result { get; set; }
  }
}