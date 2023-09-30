using MTLServiceBot.Assistants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTLServiceBot.API.Entities
{
    public class ServiceTask
    {
        private const string CommentsTag = "comments";
        private const string LineNoTag = "lineNo";
        private const string UserCommentTag = "userComment";
        private const string EpochTimeStampTag = "epochTimeStamp";

        [JsonPropertyName("Request_No")]
        public string RequestNo { get; set; } = "";
        [JsonPropertyName("Task_No")]
        public string TaskNo { get; set; } = "";
        [JsonPropertyName("StatusML")]
        public string Status { get; set; } = "";
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

        public string ToMarkedDownString()
        {   
            var sb = new StringBuilder();
            AppendMarkDownParameterLine(sb, "Запрос", RequestNo);
            AppendMarkDownParameterLine(sb, "Задача", TaskNo);
            AppendMarkDownParameterLine(sb, "Статус", Status);
            AppendMarkDownParameterLine(sb, "Исполнитель", Executor);
            AppendMarkDownParameterLine(sb, "Наименование", Name);
            AppendMarkDownParameterLine(sb, "Адрес", Address);
            AppendMarkDownParameterLine(sb, "Заказ", OrderNo);
            AppendMarkDownParameterLine(sb, "Оборудование", Equipment);
            AppendMarkDownParameterLine(sb, "Серийный Номер", SerialNumber);
            AppendMarkDownParameterLine(sb, "Описание", ServiceRequestDescription);
            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendJoin("; ", RequestNo, TaskNo);
            return sb.ToString();
        }

        private void AppendMarkDownParameterLine(StringBuilder sb, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
                sb.AppendLine($"<code>{name}:</code> {value}");
        }

        private void WriteProperty(JsonWriter writer, string name, string value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }
    }
}
