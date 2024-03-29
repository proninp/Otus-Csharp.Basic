﻿using MTLServiceBot.Assistants;
using MTLServiceBot.Bot.Commands.ServiceRequest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Telegram.Bot.Types;

namespace MTLServiceBot.API.Entities
{
    public class ServiceTask
    {
        private const string CommentsTag = "comments";
        private const string LineNoTag = "lineNo";
        private const string UserCommentTag = "userComment";
        private const string EpochTimeStampTag = "epochTimeStamp";

        public string Id { get => $"{RequestNo}{AppConfig.Instance.SingleTaskNumberFormatSeparator}{TaskNo}"; }
        public ServiceTaskStatus TaskStatus
        {
            get
            {
                if (Enum.TryParse(Status, out ServiceTaskStatus status))
                    return status;
                return ServiceTaskStatus.Error;
            }
        }

        [JsonPropertyName("Request_No")]
        public string RequestNo { get; set; } = "";
        [JsonPropertyName("Task_No")]
        public string TaskNo { get; set; } = "";
        [JsonPropertyName("Status")]
        public string Status { get; set; } = "";
        [JsonPropertyName("StatusML")]
        public string StatusML { get; set; } = "";
        [JsonPropertyName("Owner")]
        public string Owner { get; set; } = "";
        [JsonPropertyName("Owner_Name")]
        public string OwnerName { get; set; } = "";
        [JsonPropertyName("Executor")]
        public string Executor { get; set; } = "";
        [JsonPropertyName("Executors")]
        public string Executors { get; set; } = "";
        [JsonPropertyName("Short_Name")]
        public string ShortName { get; set; } = "";
        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";
        [JsonPropertyName("City")]
        public string City { get; set; } = "";
        [JsonPropertyName("Address")]
        public string Address { get; set; } = "";
        [JsonPropertyName("Order_No")]
        public string OrderNo { get; set; } = "";
        [JsonPropertyName("Equipment")]
        public string Equipment { get; set; } = "";
        [JsonPropertyName("Serial_Number")]
        public string SerialNumber { get; set; } = "";
        [JsonPropertyName("ServiceRequestDescription")]
        public string ServiceRequestDescription { get; set; } = "";
        [JsonPropertyName("AttachedFilesCount")]
        public int AttachedFilesCount { get; set; } = 0;
        public ServiceTaskCommentInfo? CommentInfo { get; set; }

        public ServiceTask() { }

        public ServiceTask(string requestNo, string taskNo, string status, string executor = "",
            string executors = "", string shortName = "", string name = "", string city = "",
            string address = "", string orderNo = "", string equipment = "", string serialNumber = "",
            string serviceRequestDescription = "", int attachedFilesCount = 0)
        {
            RequestNo = requestNo;
            TaskNo = taskNo;
            StatusML = status;
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

        public JsonContent GetNewStatusContent()
        {
            string newStatus = TaskStatus switch
            {
                ServiceTaskStatus.New => ServiceTaskStatus.Execution.ToString(),
                ServiceTaskStatus.Execution => ServiceTaskStatus.Closed.ToString(),
                _ => Status
            };

            return JsonContent.Create(new
            {
                serviceRequestNo = RequestNo,
                serviceTaskNo = TaskNo,
                newStatus = newStatus
            });
        }

        public Stream? GetNewFileStreamContent(Stream fileStream, string fileName, string fileDescription)
        {
            var cryptoStream = new CryptoStream(fileStream, new ToBase64Transform(), CryptoStreamMode.Read);
            var jsonBuilder = new Utf8JsonStreamBuilder();

            jsonBuilder.AddStartObject();
            jsonBuilder.AddString("serviceRequestNo", RequestNo);
            jsonBuilder.AddString("fileName", fileName);
            jsonBuilder.AddString("fileDescription", fileDescription);
            jsonBuilder.AddStream("fileContent", cryptoStream);
            jsonBuilder.AddEndObject();

            var requestStream = jsonBuilder.GetStream();
            return requestStream;
        }

        public JsonContent GetNewNetworkFileContent(string fileName, string filePath, string fileDescription)
        {
            return JsonContent.Create(new
            {
                serviceRequestNo = RequestNo,
                fileName,
                fileDescription,
                filePath,
            });
        }

        public string ToMarkedDownString()
        {
            var sb = new StringBuilder();
            AppendMarkDownParameterLine(sb, "Запрос", RequestNo);
            AppendMarkDownParameterLine(sb, "Задача", TaskNo);
            AppendMarkDownParameterLine(sb, "Статус", StatusML);
            AppendMarkDownParameterLine(sb, "Владелец", new String[] { Owner, OwnerName});
            AppendMarkDownParameterLine(sb, "Исполнитель", Executor);
            AppendMarkDownParameterLine(sb, "Наименование", Name);
            AppendMarkDownParameterLine(sb, "Адрес", Address);
            AppendMarkDownParameterLine(sb, "Заказ", OrderNo);
            AppendMarkDownParameterLine(sb, "Оборудование", Equipment);
            AppendMarkDownParameterLine(sb, "Серийный Номер", SerialNumber);
            AppendMarkDownParameterLine(sb, "Описание", ServiceRequestDescription);
            AppendMarkDownParameterLine(sb, "Добавлено файлов", AttachedFilesCount);
            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendJoin("; ", RequestNo, TaskNo);
            return sb.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ServiceTask task)
                return false;
            return task.Id == Id;
        }

        public override int GetHashCode() => RequestNo.GetHashCode() ^ TaskNo.GetHashCode();

        public string GetNextStatusStepDescription() => TaskStatus switch
        {
            ServiceTaskStatus.New => "В работу",
            ServiceTaskStatus.Execution => "Закрыть",
            _ => ""
        };
        private void AppendMarkDownParameterLine(StringBuilder sb, string name, string[] values)
        {
            if (values.All(s => string.IsNullOrEmpty(s)))
                return;
            if (string.IsNullOrEmpty(OwnerName))
                sb.AppendLine($"{name}: <code>{Owner}</code>");
            else if (string.IsNullOrEmpty(Owner))
                sb.AppendLine($"{name}: <code>{OwnerName}</code>");
            else
                sb.AppendLine($"{name}: <code>{OwnerName} ({Owner})</code>");
        }

        private void AppendMarkDownParameterLine(StringBuilder sb, string name, int value)
        {
            if (value > 0)
                AppendMarkDownParameterLine(sb, name, value.ToString());
        }

        private void AppendMarkDownParameterLine(StringBuilder sb, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
                sb.AppendLine($"{name}: <code>{value}</code>");
        }
    }
}
