using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTLServiceBot.API.Entities
{
    public class ServiceTaskComment
    {
        [JsonPropertyName("masterText")]
        public string? CommentText { get; set; }
        [JsonPropertyName("userCommentShort")]
        public string? UserCommentText { get; set; }
        [JsonPropertyName("commentDT")]
        public string? CommentDatetime { get; set; }
        [JsonPropertyName("userCode")]
        public string? UserCode { get; set; }
        [JsonPropertyName("userCommentLong")]
        public string? CommentTextLong { get; set; }
        public ServiceTaskComment() { }
        
        public ServiceTaskComment(string? commentText, string? userCommentText = "",
            string? commentDatetime = "", string? userCode = "", string? commentTextLong = "")
        {
            CommentText = commentText;
            UserCommentText = userCommentText;
            CommentDatetime = commentDatetime;
            UserCode = userCode;
            CommentTextLong = commentTextLong;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(CommentTextLong))
                return CommentTextLong;
            if (!string.IsNullOrEmpty(CommentText))
                return CommentText;
            if (!string.IsNullOrEmpty(UserCommentText))
                return UserCommentText;
            return "";
        }
    }
}
