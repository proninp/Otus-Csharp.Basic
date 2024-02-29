using MTLServiceBot.Assistants;

namespace MTLServiceBot.API
{
    public class ApiResponse
    {
        private readonly string _responseText;
        private readonly ApiResponseStatus _status;

        public string ResponseText { get => _responseText; }
        public ApiResponseStatus Status { get => _status; }
        public string Message { get; set; }
        public bool IsSuccess { get => _status == ApiResponseStatus.Success; }

        public ApiResponse(ApiResponseStatus status, string response, string message) : this(status, response)
        {
            Message = message;
        }

        public ApiResponse(ApiResponseStatus status, string response)
        {
            _status = status;
            _responseText = response;
            Message = GetApiResponseMessage();
        }

        private string GetApiResponseMessage()
        {
            if (_status == ApiResponseStatus.Success)
                return string.Empty;
            else if (_status == ApiResponseStatus.Unauthorized)
                return AppConfig.Instance.UnauthorizedError;

            return AppConfig.Instance.ServerConnectionError;
        }
    }
}
