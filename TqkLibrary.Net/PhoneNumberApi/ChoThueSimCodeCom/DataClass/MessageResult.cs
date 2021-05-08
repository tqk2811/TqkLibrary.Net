namespace TqkLibrary.Net.PhoneNumberApi.ChoThueSimCodeCom
{
  public class MessageResult
  {
    public string SMS { get; set; }
    public string Code { get; set; }
    public double Cost { get; set; }
    public bool IsCall { get; set; }
    public string CallFile { get; set; }
    public string CallText { get; set; }
  }
}