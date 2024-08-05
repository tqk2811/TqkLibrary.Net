using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class UrlBuilder
    {
        string _url = string.Empty;
        readonly NameValueCollection _nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public UrlBuilder(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
            this._url = url;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urls"></param>
        public UrlBuilder(params string[] urls)
        {
            if (urls == null || urls.Length == 0) throw new ArgumentNullException(nameof(urls));
            this._url = string.Join("/", urls.Select(x => x.TrimStart('/').TrimEnd('/')));
            if (string.IsNullOrWhiteSpace(_url)) throw new ArgumentNullException(nameof(_url));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urls"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UrlBuilder(params object[] urls)
        {
            if (urls == null || urls.Length == 0) throw new ArgumentNullException(nameof(urls));
            this._url = string.Join("/", urls.Select(x => x.ToString()?.TrimStart('/').TrimEnd('/') ?? throw new ArgumentNullException($"Some element in {nameof(urls)} are null")));
            if (string.IsNullOrWhiteSpace(_url)) throw new ArgumentNullException(nameof(_url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual UrlBuilder WithParam(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            string? v = value?.ToString();
            if (string.IsNullOrWhiteSpace(v)) throw new ArgumentNullException(nameof(value));
            _nameValueCollection.Add(name, v);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UrlBuilder WithParam(string name, IEnumerable<object> values)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (values is null || !values.Any()) throw new ArgumentNullException(nameof(values));
            foreach (object value in values) WithParam(name, value?.ToString()!);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual UrlBuilder WithParams(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection is null) throw new ArgumentNullException(nameof(nameValueCollection));
            this._nameValueCollection.Add(nameValueCollection);
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual UrlBuilder WithParamIfNotNull(string name, object? value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (value != null)
            {
                string? v = value.ToString();
                if (!string.IsNullOrWhiteSpace(v)) _nameValueCollection.Add(name, v);
            }
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UrlBuilder WithParamIfNotNull(string name, IEnumerable<object?>? values)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (values is null || !values.Any()) return this;
            foreach (object? value in values) WithParamIfNotNull(name, value?.ToString());
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="condition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UrlBuilder WithParamIf(string name, Func<bool> condition, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (condition is null) throw new ArgumentNullException(nameof(condition));
            if (condition.Invoke())
            {
                this.WithParam(name, value);
            }
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="condition"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UrlBuilder WithParamIf(string name, Func<bool> condition, IEnumerable<object> values)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (condition is null) throw new ArgumentNullException(nameof(condition));
            if (condition.Invoke())
            {
                this.WithParam(name, values);
            }
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string? query = _nameValueCollection.ToString();
            if (string.IsNullOrWhiteSpace(query)) return _url;
            else return $"{_url}?{_nameValueCollection}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static explicit operator Uri(UrlBuilder builder) => new Uri(builder.ToString());

    }
}
