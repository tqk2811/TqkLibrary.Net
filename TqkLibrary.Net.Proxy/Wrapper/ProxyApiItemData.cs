using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace TqkLibrary.Net.Proxy.Wrapper
{
    internal class ProxyApiItemData
    {
        readonly AsyncLock _asyncLock = new AsyncLock();
        public string CurrentProxy { get; private set; } = string.Empty;
        public DateTime NextReset { get; set; } = DateTime.Now;
        public DateTime ExpiredTime { get; private set; } = DateTime.Now.AddYears(10);
        public int UsedCount { get; private set; } = 0;
        public int UsingCount { get; private set; } = 0;
        public ProxyType ProxyType { get; private set; } = ProxyType.Invalid;
        public void Reset(IProxyApiResponseWrapper proxyApiResponse)
        {
            if (proxyApiResponse == null) throw new ArgumentNullException(nameof(proxyApiResponse));
            NextReset = proxyApiResponse.NextTime;
            if (proxyApiResponse.IsSuccess)
            {
                UsedCount = 0;
                UsingCount = 0;
                CurrentProxy = proxyApiResponse.Proxy!;
                ExpiredTime = proxyApiResponse.ExpiredTime;
                ProxyType = proxyApiResponse.ProxyType;
            }
        }
        public void AddRef()
        {
            using var l = _asyncLock.Lock();
            UsingCount++;
            UsedCount++;
        }
        public async Task AddRefAsync()
        {
            using var l = await _asyncLock.LockAsync().ConfigureAwait(false);
            UsingCount++;
            UsedCount++;
        }
        public void RemoveRef()
        {
            using var l = _asyncLock.Lock();
            UsingCount--;
        }
        public async Task RemoveRefAsync()
        {
            using var l = await _asyncLock.LockAsync().ConfigureAwait(false);
            UsingCount--;
        }
    }
}
