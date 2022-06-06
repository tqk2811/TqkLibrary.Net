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
        string url = string.Empty;
        readonly NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public UriBuilder(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            this.url = url;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urls"></param>
        public UriBuilder(params string[] urls)
        {
            if (urls == null || urls.Length == 0) throw new ArgumentNullException(nameof(urls));
            this.url = string.Join("/", urls.Select(x => x.TrimStart('/').TrimEnd('/')));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urls"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UriBuilder(params object[] urls)
        {
            if (urls == null || urls.Length == 0) throw new ArgumentNullException(nameof(urls));
            this.url = string.Join("/", urls.Select(x => x.ToString().TrimStart('/').TrimEnd('/')));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
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
        public override string ToString()
        {
            string query = nameValueCollection.ToString();
            if (string.IsNullOrWhiteSpace(query)) return url;
            else return $"{url}?{nameValueCollection}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static explicit operator Uri(UriBuilder builder) => new Uri(builder.ToString());
    }
}
