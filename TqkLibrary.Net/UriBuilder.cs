using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Collections.Specialized;

namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class UriBuilder
    {
        readonly NameValueCollection nameValueCollection;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public UriBuilder(string url)
        {
            nameValueCollection = HttpUtility.ParseQueryString(url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urls"></param>
        public UriBuilder(params string[] urls)
        {
            nameValueCollection = HttpUtility.ParseQueryString(string.Join("", urls));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UriBuilder WithParam(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            nameValueCollection[name] = value;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UriBuilder WithParam(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            string v = value?.ToString();
            if (string.IsNullOrWhiteSpace(v)) throw new ArgumentNullException(nameof(value));
            nameValueCollection[name] = v;
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UriBuilder WithParamIfNotNull(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (!string.IsNullOrWhiteSpace(value)) nameValueCollection[name] = value;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UriBuilder WithParamIfNotNull(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (value != null)
            {
                string v = value.ToString();
                if (!string.IsNullOrWhiteSpace(v)) nameValueCollection[name] = v;
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => nameValueCollection.ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static explicit operator Uri(UriBuilder builder) => new Uri(builder.ToString());
    }
}
