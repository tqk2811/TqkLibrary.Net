using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Cryptos
{
  public class PancakeSwapBaseResponse<T>
  {
    public long updated_at { get; set; }
    public T data { get; set; }
  }
}
