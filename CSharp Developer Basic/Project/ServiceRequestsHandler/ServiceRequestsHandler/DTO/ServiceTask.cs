using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceRequestsHandler.API;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace ServiceRequestsHandler.DTO
{
    public class ServiceTask
    {
        private const string CommentsTag = "comments";
        private const string LineNoTag = "lineNo";
        private const string UserCommentTag = "userComment";
        private const string EpochTimeStampTag = "epochTimeStamp";

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
        public ServiceTaskCommentInfo? CommentInfo { get; set; }
        public List<ServiceTaskFilesInfo>? FilesList { get; set; }


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

        public string GetUriFilter() => $"?$filter=Request_No eq '{RequestNo}' and Task_No eq '{TaskNo}'";

        public string GetTaskCommetsUrl() => $"{Config.ServiceTasksApiUrl}('{RequestNo}', '{TaskNo}')/NAV.GetRequestComments";

        public string AddTaskCommetsUrl() => $"{Config.ServiceTasksApiUrl}('{RequestNo}', '{TaskNo}')/NAV.AddRequestComments";

        public string GetServiceTaskFilesListUtl() => $"{Config.SetTaskFilesListUrl}?$filter=requestNo eq '{RequestNo}'";

        public JsonContent GetNewStatusContent(ServiceTaskStatus newStatus)
        {
            return JsonContent.Create(new
            {
                serviceRequestNo = RequestNo,
                serviceTaskNo = TaskNo,
                newStatus = newStatus.ToString()
            });
        }

        public JsonContent GetNewCommentContent(string comment)
        {
            return JsonContent.Create(new
            {
                requestNo = RequestNo,
                taskNo = TaskNo,
                comment = comment,
                epochTimeStamp = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString()
            });
        }

        public JsonContent GetNewCommentsContent(List<string> comments)
        {
            var jComments = new JObject();
            var jArray = new JArray();
            JObject jLine;
            foreach (var comment in comments)
            {
                jLine = new JObject();
                var writer = jLine.CreateWriter();
                WriteProperty(writer, LineNoTag, "");
                WriteProperty(writer, UserCommentTag, comment);
                WriteProperty(writer, EpochTimeStampTag,
                    ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString());
                jArray.Add(jLine);
            }
            jComments.Add(CommentsTag, jArray);
            var jsonContent = new JObject();
            jsonContent.Add("jsonText", jComments.ToString().Replace('"', '\u0022'));
            return JsonContent.Create(jsonContent);
        }


        public void AddCommentsFromJsonResponse(ApiResponse? commentsResponse)
        {
            if (commentsResponse == null)
                return;
            var commentsJson = commentsResponse.ToString();
            var stCommentsList = new List<ServiceTaskComment>();
            try
            {
                CommentInfo = System.Text.Json.JsonSerializer.Deserialize<ServiceTaskCommentInfo>(commentsJson);
                if (int.TryParse(CommentInfo?.CommentsCount, out int commentsCount) && commentsCount > 0)
                {
                    JObject commentsJObjet = JObject.Parse(commentsJson);
                    if (commentsJObjet is not null && commentsJObjet.ContainsKey(CommentsTag))
                    {
                        string jsonCommentsInfoText = commentsJObjet[CommentsTag].ToString();
                        stCommentsList = System.Text.Json.JsonSerializer.Deserialize<List<ServiceTaskComment>>(jsonCommentsInfoText);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ColoredPrint($"Не удалось осуществить десериализацию данных по комментариям для сервисного задания {RequestNo} {TaskNo}", ConsoleColor.Red);
                #if DEBUG
                Program.ColoredPrint($"Ошабка:\n{ex.Message}");
                #endif
                // TODO Log

            }
            if (CommentInfo is not null)
                CommentInfo.Comments = stCommentsList;
            
        }
        public void AddFilesInfoFromJsonResponse(ApiResponse? filesListResponse)
        {
            if (filesListResponse == null)
                return;
            if (filesListResponse.Status == ResponseStatus.Success)
            {
                List<ServiceTaskFilesInfo>? filesList = null;
                try
                {
                    filesList = System.Text.Json.JsonSerializer.Deserialize<List<ServiceTaskFilesInfo>>(filesListResponse.ToString());
                }
                catch (Exception ex)
                {
                    Program.ColoredPrint($"Не удалось осуществить десериализацию данных по информации о файлах сервисного обращения {RequestNo}", ConsoleColor.Red);
                    #if DEBUG
                    Program.ColoredPrint($"Ошабка:\n{ex.Message}");
                    #endif
                    // TODO Log
                }
                FilesList = filesList;
                if (FilesList == null)
                    Program.ColoredPrint($"Нет информации по файлам для заявки {RequestNo}");
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Request: {RequestNo ?? ""};");
            sb.AppendLine($"Task: {TaskNo ?? ""};");
            sb.AppendLine($"Status: {Status ?? ""};");
            sb.AppendLine($"nExecutor: {Executor ?? ""};");
            sb.AppendLine($"ShortName: {ShortName ?? ""};");
            sb.AppendLine($"Name: {Name ?? ""};");
            sb.AppendLine($"City: {City ?? ""};");
            sb.AppendLine($"Address: {Address ?? ""};");
            sb.AppendLine($"OrderNo: {OrderNo ?? ""};");
            sb.AppendLine($"Equipment: {Equipment ?? ""};");
            sb.AppendLine($"SerialNumber: {SerialNumber ?? ""};");
            sb.AppendLine($"ServiceRequestDescription: {ServiceRequestDescription ?? ""};");
            sb.AppendLine($"Комментарии по задаче:");
            if (CommentInfo is not null)
                sb.AppendLine($"{CommentInfo.ToString()}");
            return sb.ToString();
        }

        private void WriteProperty(JsonWriter writer, string name, string value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }
    }
}
