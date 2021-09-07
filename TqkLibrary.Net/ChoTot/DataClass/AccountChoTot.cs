#if NET462_OR_GREATER
namespace TqkLibrary.Net.ChoTot
{
  public class AccountChoTot
  {
    public string Phone { get; set; }
    public string Pass { get; set; }

    public override bool Equals(object obj)
    {
      if(obj is AccountChoTot accountChoTot)
      {
        return  !string.IsNullOrEmpty(Phone) && Phone.Equals(accountChoTot.Phone);
      }
      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
#endif