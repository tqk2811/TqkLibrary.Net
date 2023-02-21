#if NET462_OR_GREATER
using System.Collections.Generic;

namespace TqkLibrary.Net.Ecommerce.ChoTot
{
    public class Wards
    {
        public string code { get; set; }
        public List<WardItem> wards { get; set; }
    }
    public class WardItem
    {
        public string name { get; set; }
        public int id { get; set; }
    }
}
#endif