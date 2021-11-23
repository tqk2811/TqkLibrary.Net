using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Translate
{
  public enum TransLanguage
  {
    auto,
    en,
    vi,

  }
  public class GoogleTrans : BaseApi
  {
    public GoogleTrans(CancellationToken cancellationToken) : base(cancellationToken)
    {

    }

    public Task<string> Translate(string text,TransLanguage from,TransLanguage to)
    {
      return this.RequestGet<string>($"https://translate.googleapis.com/translate_a/single?client=gtx&sl={from}&tl={to}&dt=t&q=%22{text}%22");
    }
  }
}
