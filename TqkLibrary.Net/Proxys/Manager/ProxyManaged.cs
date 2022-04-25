using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TqkLibrary.Net.Proxys.Manager
{

    /// <summary>
    /// 
    /// </summary>
    public class ProxyManaged
    {
        readonly Dictionary<IProxyApi, ProxyApiItemData> _dicts = new Dictionary<IProxyApi, ProxyApiItemData>();
        readonly AsyncLock asyncLock = new AsyncLock();
        /// <summary>
        /// 
        /// </summary>
        public int MaxUseCountPerApi { get; set; } = int.MaxValue;
        /// <summary>
        /// 
        /// </summary>
        public int Delay { get; set; } = 100;

        /// <summary>
        /// Allow proxy life-time left greater than this<br>
        /// </br>set to TimeSpan.Zero is un-limit<br>
        /// </br>Default: TimeSpan.Zero
        /// </summary>
        public TimeSpan AllowTimeLeft { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// 
        /// </summary>
        public async Task Load(IEnumerable<IProxyApi> proxyApis, bool isClear = true)
        {
            var apis = proxyApis?.ToList() ?? throw new ArgumentNullException(nameof(proxyApis));
            if (apis.Any(x => x == null)) throw new ArgumentNullException($"Have item in {nameof(proxyApis)} null");
            using var l = await asyncLock.LockAsync().ConfigureAwait(false);
            if (isClear) _dicts.Clear();
            foreach (var api in apis) _dicts[api] = new ProxyApiItemData();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            using var l = await asyncLock.LockAsync().ConfigureAwait(false);
            _dicts.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IProxy> GetProxyAsync(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (await asyncLock.LockAsync(cancellationToken).ConfigureAwait(false))
                {
                    if (_dicts.Count == 0) return null;
                    
                    var currTime = DateTime.Now;
                    //check is item can reset
                    var pair = _dicts.FirstOrDefault(x =>
                        currTime > x.Value.NextReset &&
                        (x.Key.IsAllowGetNewOnUsing || x.Value.UsingCount == 0) &&
                        (AllowTimeLeft == TimeSpan.Zero || currTime + AllowTimeLeft < x.Value.ExpiredTime));
                    if (pair.Key != null)
                    {
                        IProxyApiResponse proxyApiResponse = await pair.Key.GetNewProxyAsync(cancellationToken).ConfigureAwait(false);
                        if (proxyApiResponse == null) throw new InvalidOperationException(nameof(proxyApiResponse));
                        pair.Value.Reset(proxyApiResponse);

                        if (proxyApiResponse.IsSuccess == true)
                        {
                            return new Proxy_(pair.Value);
                        }
                    }
                    else
                    {
                        //check is item allow using more times
                        pair = _dicts.FirstOrDefault(x => MaxUseCountPerApi > x.Value.UsingCount);
                        if (pair.Key != null)
                        {
                            return new Proxy_(pair.Value);
                        }
                    }
                }

                await Task.Delay(Delay, cancellationToken);
            }
        }
    }
}
