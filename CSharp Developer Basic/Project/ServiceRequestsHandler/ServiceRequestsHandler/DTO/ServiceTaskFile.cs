using System.Text.Json.Serialization;

namespace ServiceRequestsHandler.DTO
{
    public class ServiceTaskFile
    {
        [JsonPropertyName("fileName")]
        public string? FileName { get; set; }
        [JsonPropertyName("fileGUID")]
        public string? FileGuid { get; private set; }
        [JsonPropertyName("fileDescription")]
        public string? FileDescription { get; set; }
        [JsonPropertyName("fileDateTime")]
        public string? FileDateTime { get; set; }
        [JsonPropertyName("fileContent")]
        public string? Base64Content { get; set; }
        public ServiceTaskFile() { }
        public ServiceTaskFile(string? fileName, string? fileGuid = "",
            string? fileDescription = "", string? fileDateTime = "", string? base64Content = "")
        {
            FileName = fileName;
            FileGuid = fileGuid;
            FileDescription = fileDescription;
            FileDateTime = fileDateTime;
            Base64Content = base64Content;
        }
        public override string ToString()
        {
            return $"{FileName}";
        }
    }
}
