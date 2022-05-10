﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System;
namespace TqkLibrary.Net.Others.FptAi
{
    public class TTSResponse
    {
        public int error { get; set; }
        public string async { get; set; }
        public string request_id { get; set; }
        public string message { get; set; }

        public async Task<byte[]> Download(int timeout = 30000, int step = 2000, CancellationToken cancellationToken = default)
        {
            using HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, this.async);
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(timeout))
            {
                while (true)
                {
                    await Task.Delay(step, cancellationToken).ConfigureAwait(false);
                    using HttpResponseMessage httpResponseMessage = await NetSingleton.httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    if (httpResponseMessage.IsSuccessStatusCode) return await httpResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    if (cancellationTokenSource.IsCancellationRequested) return await httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
