namespace MTLServiceBot.API
{
    public class ApiResponse
    {
        public string ResponseText { get; set; }
        public ApiResponseStatus Status { get; set; }
        public ApiResponse(ApiResponseStatus status, string response)
        {
            Status = status;
            ResponseText = response;
        }
    }
}
