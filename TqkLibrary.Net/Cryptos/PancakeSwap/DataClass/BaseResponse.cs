using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Cryptos.PancakeSwap
{
  public class BaseResponse<T>
  {
    public long updated_at { get; set; }
    public T data { get; set; }
  }
}
