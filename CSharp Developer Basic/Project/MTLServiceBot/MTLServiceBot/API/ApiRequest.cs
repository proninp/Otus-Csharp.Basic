namespace MTLServiceBot.API
{
    public class ApiRequest
    {
        private readonly HttpContent? _httpContent;
        private readonly Stream? _contentStream;

        public string Url { get; set; }
        public HttpMethod Method { get; set; }
        public string AuthHeader { get; set; }
        public HttpContent? HttpContent { get => _httpContent; }
        public Stream? ContentStream { get => _contentStream; }

        public ApiRequest(string url, HttpMethod method, string authHeader, Stream? contentStream) : this(url, method, authHeader)
        {
            _contentStream = contentStream;
        }

        public ApiRequest(string url, HttpMethod method, string authHeader, HttpContent? httpContent): this(url, method, authHeader)
        {
            _httpContent = httpContent;
        }

        public ApiRequest(string url, HttpMethod method, string authHeader)
        {
            Url = url;
            Method = method;
            AuthHeader = authHeader;
        }
    }
}
