using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TqkLibrary.Net.CloudStorage.GoogleDrive
{
    internal class StreamWrapper : Stream
    {
        readonly HttpResponseMessage httpResponseMessage;
        readonly Stream stream;
        internal StreamWrapper(HttpResponseMessage httpResponseMessage, Stream stream)
        {
            this.httpResponseMessage = httpResponseMessage ?? throw new ArgumentNullException(nameof(httpResponseMessage));
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }
        ~StreamWrapper()
        {
            Dispose(false);
        }
        protected override void Dispose(bool disposing)
        {
            httpResponseMessage.Dispose();
            stream.Dispose();
            base.Dispose(disposing);
        }
        public override bool CanRead => stream.CanRead;

        public override bool CanSeek => stream.CanSeek;

        public override bool CanWrite => stream.CanWrite;

        public override long Length => httpResponseMessage.Content?.Headers?.ContentLength ?? stream.Length;

        public override long Position { get => stream.Position; set => stream.Position = value; }

        public override void Flush()
        {
            stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            stream.Write(buffer, offset, count);
        }


        //https://devblogs.microsoft.com/pfxteam/overriding-stream-asynchrony/
        //must overwite BeginRead/EndRead, BeginWrite/EndWrite for asynchronous 
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return stream.BeginRead(buffer, offset, count, callback, state);
        }
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return stream.BeginWrite(buffer, offset, count, callback, state);
        }
        public override int EndRead(IAsyncResult asyncResult)
        {
            return stream.EndRead(asyncResult);
        }
        public override void EndWrite(IAsyncResult asyncResult)
        {
            stream.EndWrite(asyncResult);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            return stream.ReadAsync(buffer, offset, count, cancellationToken);
        }
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            return stream.WriteAsync(buffer, offset, count, cancellationToken);
        }
        public override Task FlushAsync(CancellationToken cancellationToken = default)
        {
            return stream.FlushAsync(cancellationToken);
        }
#if NET5_0_OR_GREATER
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return stream.ReadAsync(buffer, cancellationToken);
        }
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return stream.WriteAsync(buffer, cancellationToken);
        }
#endif
    }
}