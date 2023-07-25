namespace ServiceRequestsHandler.DTO
{
    public class ServiceTaskFile
    {
        public string? FileName { get; set; }
        public string? FileGuid { get; private set; }
        public string? FileDescription { get; set; }
        public string? FileDateTime { get; set; }
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
    }
}
