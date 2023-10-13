using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTLServiceBot.API
{
    public class ApiRequest
    {
        public string AuthHeader { get; set; }
        HttpMethod Method { get; set; }
        string Url { get; set; }
        HttpContent? Content { get; set; } = null;
        Stream? DataStream { get; set; } = null;

        public ApiRequest(string authHeader, HttpMethod method, string url, HttpContent? content = null, Stream? dataStream = null)
        {
            AuthHeader = authHeader;
            Method = method;
            Url = url;
            Content = content;
            DataStream = dataStream;
        }
    }
}
