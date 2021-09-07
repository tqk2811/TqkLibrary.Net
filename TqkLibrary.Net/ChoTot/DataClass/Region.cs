#if NET462_OR_GREATER
using System.Collections.Generic;

namespace TqkLibrary.Net.ChoTot
{
  public class Region : Zone
  {
    public List<Zone> Zone { get; set; }
  }
}
#endif