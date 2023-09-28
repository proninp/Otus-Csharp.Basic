using MTLServiceBot.Bot;
using Telegram.Bot.Types;

namespace MTLServiceBot.API
{
    public class ApiResponse
    {
        private readonly string _responseText;
        public string ResponseText { get => _responseText; }
        public string Message { get; set; }
        private readonly ApiResponseStatus _status;
        public ApiResponseStatus Status { get => _status; }
        public bool IsSuccess { get => _status == ApiResponseStatus.Success; }

        public ApiResponse(ApiResponseStatus status, string response, string message): this(status, response)
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
                return "";
            else if (_status == ApiResponseStatus.Unauthorized)
                return TextConsts.UnauthorizedError;
            else
                return TextConsts.ServerConnectionError;
        }
    }
}
