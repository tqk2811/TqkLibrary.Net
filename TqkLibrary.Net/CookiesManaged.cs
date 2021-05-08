using System.Collections.Generic;
using System.Linq;

namespace TqkLibrary.Net
{
  public static class CookiesManaged
  {
    public static Dictionary<string, string> Parse(this IEnumerable<string> Cookies, Dictionary<string, string> Containers = null)
    {
      if (Containers == null) Containers = new Dictionary<string, string>();
      foreach (string cookie in Cookies)
      {
        string[] carr = cookie.Split(';');
        if (carr.Length >= 1)
        {
          string[] c = carr[0].Split('=');
          if (c.Length == 2)
          {
            if (Containers.ContainsKey(c[0])) Containers.Remove(c[0]);
            Containers.Add(c[0], c[1]);
          }
        }
      }
      return Containers;
    }

    public static string GetCookiesString(this Dictionary<string, string> Containers) => string.Join(";", Containers.Select(x => x.Key + "=" + x.Value));
  }
}