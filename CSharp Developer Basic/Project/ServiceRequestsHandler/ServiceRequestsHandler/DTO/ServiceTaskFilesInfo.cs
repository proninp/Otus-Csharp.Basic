using System.Text.Json.Serialization;

namespace ServiceRequestsHandler.DTO
{
    public class ServiceTaskFilesInfo
    {
        [JsonPropertyName("requestNo")]
        public string? RequestNo { get; set; }
        [JsonPropertyName("lineNo")]
        public int LineNo { get; set; }
        [JsonPropertyName("executor")]
        public string? Executor { get; set; }
        [JsonPropertyName("fileName")]
        public string? FileName { get; set; }
        [JsonPropertyName("fileDescription")]
        public string? FileDescription { get; set; }
        [JsonPropertyName("fileHash")]
        public string? FileHash { get; set; }
        [JsonPropertyName("fileDateTime")]
        public string? FileDateTime { get; set; }
        public string? FileBase64Content { get; set; }
        public override string ToString()
        {
            return $"Filename: {FileName}; Filehash: {FileHash}; File datetime: {FileDateTime}";
        }
    }
}
