using System.Collections;

namespace MTLServiceBot.Assistants
{
    public class SequenceReadOnlyStream : Stream
    {
        private readonly IEnumerable<Stream> _streams;
        private readonly IEnumerator _enumerator;

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException("Stream does not support seeking.");

        public override long Position
        {
            get => throw new NotSupportedException("Stream does not support seeking.");
            set => throw new NotSupportedException("Stream does not support seeking.");
        }

        public SequenceReadOnlyStream(params Stream[] streams)
        {
            _streams = streams.AsEnumerable();
            _enumerator = streams.GetEnumerator();
            _enumerator.MoveNext();
        }

        public override void Flush()
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_enumerator.Current is not Stream currentStream)
            {
                _enumerator.MoveNext();
                currentStream = (Stream)_enumerator.Current;
            }

            while (currentStream != null)
            {
                var readed = currentStream.Read(buffer, offset, count);
                if (readed == 0)
                {
                    if (_enumerator.MoveNext())
                    {
                        currentStream = (Stream)_enumerator.Current;
                        continue;
                    }
                    break;
                }
                return readed;
            }
            return 0;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("Stream does not support seeking.");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Stream does not support seeking.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Stream does not support writing.");
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;

            foreach (var stream in _streams)
            {
                stream.Dispose();
            }
        }
    }
}
