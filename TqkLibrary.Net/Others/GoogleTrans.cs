using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.Others
{

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum TransLanguage
    {
        auto,
        en,
        vi,

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// 
    /// </summary>
    public class GoogleTrans : BaseApi
    {
        /// <summary>
        /// 
        /// </summary>
        public GoogleTrans() : base("no api key")
        {
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public Task<string> Translate(string text, TransLanguage from, TransLanguage to)
        {
            return this.RequestGetAsync<string>($"https://translate.googleapis.com/translate_a/single?client=gtx&sl={from}&tl={to}&dt=t&q=%22{text}%22");
        }
    }
}
