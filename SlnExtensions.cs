#if !NET5_0_OR_GREATER
using System;
namespace TqkLibrary.Net
{
    internal static class SlnExtensions
    {
        public static bool Contains(this string text, string value, StringComparison stringComparison)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }
}
#endif