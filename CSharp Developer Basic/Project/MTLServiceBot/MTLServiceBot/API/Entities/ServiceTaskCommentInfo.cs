using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTLServiceBot.API.Entities
{
    public class ServiceTaskCommentInfo
    {
        [JsonPropertyName("requestNo")]
        public string? RequestNo { get; set; }
        [JsonPropertyName("taskNo")]
        public string? TaskNo { get; set; }
        [JsonPropertyName("commentsCount")]
        public string? CommentsCount { get; set; }
        [JsonPropertyName("comments")]
        public List<ServiceTaskComment>? Comments { get; set; }
        
        public override string ToString()
        {
            if (Comments is null)
                return "";
            var sb = new StringBuilder();
            foreach (var e in Comments)
                sb.AppendLine($"\t{e}");
            return sb.ToString();
        }
    }
}
