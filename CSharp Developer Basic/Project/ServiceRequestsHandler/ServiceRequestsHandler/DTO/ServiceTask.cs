using System.Text.Json.Serialization;

namespace ServiceRequestsHandler.DTO
{
    public class ServiceTask
    {
        [JsonPropertyName("Request_No")]
        public string? RequestNo { get; set; }
        [JsonPropertyName("Task_No")]
        public string? TaskNo { get; set; }
        [JsonPropertyName("Status")]
        public string? Status { get; set; }
        [JsonPropertyName("Executor")]
        public string? Executor { get; set; }
        [JsonPropertyName("Executors")]
        public string? Executors { get; set; }
        [JsonPropertyName("Short_Name")]
        public string? ShortName { get; set; }
        [JsonPropertyName("Name")]
        public string? Name { get; set; }
        [JsonPropertyName("City")]
        public string? City { get; set; }
        [JsonPropertyName("Address")]
        public string? Address { get; set; }
        [JsonPropertyName("Order_No")]
        public string? OrderNo { get; set; }
        [JsonPropertyName("Equipment")]
        public string? Equipment { get; set; }
        [JsonPropertyName("Serial_Number")]
        public string? SerialNumber { get; set; }
        [JsonPropertyName("ServiceRequestDescription")]
        public string? ServiceRequestDescription { get; set; }
        [JsonPropertyName("AttachedFilesCount")]
        public int AttachedFilesCount { get; set; }

        public ServiceTask() { }
        
        public ServiceTask(string? requestNo, string? taskNo, string? status, string? executor = "",
            string? executors = "", string? shortName = "", string? name = "", string? city = "",
            string? address = "", string? orderNo = "", string? equipment = "", string? serialNumber = "",
            string? serviceRequestDescription = "", int attachedFilesCount = 0)
        {
            RequestNo = requestNo;
            TaskNo = taskNo;
            Status = status;
            Executor = executor;
            Executors = executors;
            ShortName = shortName;
            Name = name;
            City = city;
            Address = address;
            OrderNo = orderNo;
            Equipment = equipment;
            SerialNumber = serialNumber;
            ServiceRequestDescription = serviceRequestDescription;
            AttachedFilesCount = attachedFilesCount;
        }
    }
}
