using System.Runtime.Serialization;

namespace ServiceRequestsHandler.DTO
{
    public enum ServiceTaskStatus
    {
        [EnumMember(Value = "New")]
        New = 0,
        [EnumMember(Value = "Execution")]
        Execution = 1,
        [EnumMember(Value = "Agreement")]
        Agreement = 2,
        [EnumMember(Value = "Feedback")]
        Feedback = 3,
        [EnumMember(Value = "Closed")]
        Closed = 4,
        [EnumMember(Value = "Archive")]
        Archive = 5,
        [EnumMember(Value = "Deleted")]
        Deleted = 6,
        [EnumMember(Value = "Commercial")]
        Commercial = 7
    }
}
