namespace ServiceRequestsHandler.DTO
{
    public class ServiceTaskFilesInfo
    {
        public string? RequestNo { get; set; }
        public List<ServiceTaskFile>? Files { get; set; }
        public ServiceTaskFilesInfo(string? requestNo) : this(requestNo, new List<ServiceTaskFile>()) { }
        public ServiceTaskFilesInfo() { }
        public ServiceTaskFilesInfo(string? requestNo, List<ServiceTaskFile>? files)
        {
            RequestNo = requestNo;
            Files = files;
        }
    }
}
