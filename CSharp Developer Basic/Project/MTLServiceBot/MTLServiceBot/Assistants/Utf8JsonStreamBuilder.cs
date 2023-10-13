using System.Text;

namespace MTLServiceBot.Assistants
{
    public class Utf8JsonStreamBuilder
    {
        private List<Stream> _streams = new();
        private StreamWriter _currentStreamWriter;

        public Utf8JsonStreamBuilder()
        {
            AddStream();
        }

        public void AddStartObject()
        {
            _currentStreamWriter.Write("{");
        }

        public void AddEndObject()
        {
            _currentStreamWriter.Write("}");
        }

        public void AddString(string propertyName, string value)
        {
            _currentStreamWriter.Write(@$"""{propertyName}"":""{value}"",");
        }

        public void AddStream(string propertyName, Stream value)
        {
            _streams.Add(value);

            _currentStreamWriter.Write(@$"""{propertyName}"":""");
            AddStream();
            _currentStreamWriter.Write(@""",");
        }

        public Stream GetStream()
        {
            _currentStreamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            return new SequenceReadOnlyStream(_streams.ToArray());
        }

        private void AddStream()
        {
            _currentStreamWriter?.BaseStream.Seek(0, SeekOrigin.Begin);

            var stream = new MemoryStream();
            _streams.Add(stream);
            _currentStreamWriter = new StreamWriter(stream, new UTF8Encoding(false));
            _currentStreamWriter.AutoFlush = true;
        }
    }
}
