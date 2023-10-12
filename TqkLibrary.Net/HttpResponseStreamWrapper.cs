using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpResponseStreamWrapper : Stream
    {
        readonly HttpResponseMessage httpResponseMessage;
        readonly Stream stream;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponseMessage"></param>
        /// <param name="stream"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public HttpResponseStreamWrapper(HttpResponseMessage httpResponseMessage, Stream stream)
        {
            this.httpResponseMessage = httpResponseMessage ?? throw new ArgumentNullException(nameof(httpResponseMessage));
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }
        /// <summary>
        /// 
        /// </summary>
        ~HttpResponseStreamWrapper()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            httpResponseMessage.Dispose();
            stream.Dispose();
            base.Dispose(disposing);
        }
        /// <inheritdoc/>
        public override bool CanRead => stream.CanRead;
        /// <inheritdoc/>
        public override bool CanSeek => stream.CanSeek;
        /// <inheritdoc/>
        public override bool CanWrite => stream.CanWrite;
        /// <inheritdoc/>
        public override long Length => httpResponseMessage.Content?.Headers?.ContentLength ?? stream.Length;
        /// <inheritdoc/>
        public override long Position { get => stream.Position; set => stream.Position = value; }
        /// <inheritdoc/>
        public override void Flush()
        {
            stream.Flush();
        }
        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return stream.Read(buffer, offset, count);
        }
        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }
        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }
        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            stream.Write(buffer, offset, count);
        }


        //https://devblogs.microsoft.com/pfxteam/overriding-stream-asynchrony/
        //must overwite BeginRead/EndRead, BeginWrite/EndWrite for asynchronous 
        /// <inheritdoc/>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return stream.BeginRead(buffer, offset, count, callback, state);
        }
        /// <inheritdoc/>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return stream.BeginWrite(buffer, offset, count, callback, state);
        }
        /// <inheritdoc/>
        public override int EndRead(IAsyncResult asyncResult)
        {
            return stream.EndRead(asyncResult);
        }
        /// <inheritdoc/>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            stream.EndWrite(asyncResult);
        }
        /// <inheritdoc/>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            return stream.ReadAsync(buffer, offset, count, cancellationToken);
        }
        /// <inheritdoc/>
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            return stream.WriteAsync(buffer, offset, count, cancellationToken);
        }
        /// <inheritdoc/>
        public override Task FlushAsync(CancellationToken cancellationToken = default)
        {
            return stream.FlushAsync(cancellationToken);
        }
#if NET5_0_OR_GREATER
        /// <inheritdoc/>
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return stream.ReadAsync(buffer, cancellationToken);
        }
        /// <inheritdoc/>
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return stream.WriteAsync(buffer, cancellationToken);
        }
#endif
    }
}