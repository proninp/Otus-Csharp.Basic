using Newtonsoft.Json.Linq;

namespace MTLServiceBot.API
{
    public class ApiResponse
    {
        public ResponseStatus Status { get; private set; }
        public string ErrorText { get; private set; } = "";
        public string ResponseText { get; private set; } = "";
        public ApiResponse() { }
        public void SetDefaultException()
        {
            Status = ResponseStatus.Error;
            ErrorText = "Непредвиденная ошибка обработки запроса. Попробуйте выполнить запрос позже или обратитесь к администратору системы.";
        }
        public async Task GetApiResponse(HttpResponseMessage? httpResponse)
        {
            if (httpResponse != null)
            {
                HttpResponseMessage response = httpResponse;
                var statusCode = (int)httpResponse.StatusCode;
                if (statusCode >= 200 && statusCode < 300)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        JObject jObject = JObject.Parse(jsonResponse);
                        ResponseText = jObject["value"]?.ToString();
                        Status = ResponseStatus.Success;
                        return;
                    }
                }
            }
            SetDefaultException();
        }
        public override string ToString()
        {
            if (Status == ResponseStatus.Success)
                return ResponseText;
            return ErrorText;
        }
    }
}
