using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace MTLServiceBot.API
{
    internal class NavAPI
    {
        private static HttpClient? _httpClient;
        private HttpResponseMessage? _httpResponse;
        public string? _responseText;

        public NavAPI() 
        {
            _httpClient = new();
        }

        /// <summary>
        /// Отправка API запроса в MS Dynamics Nav
        /// </summary>
        /// <param name="authHeader">Заголовок авторизации</param>
        /// <param name="method">http метод</param>
        /// <param name="apiUrl">Адрес URL</param>
        /// <param name="content">Содержимое сообщения</param>
        /// <returns>Ответ от сервера MS Dynamics Nav</returns>
        public async Task<(ApiResponseStatus status, string responseText)> SendServiceApiRequest(string authHeader, HttpMethod method, string apiUrl, JsonContent? content = null)
        {
            _responseText = string.Empty;
            try
            {
                _httpResponse = await SendApiRequset(authHeader, method, apiUrl, content);
                var task = GetApiResponse();
            }
            catch (Exception ex)
            {
                Program.ColoredPrint(ex.ToString(), ConsoleColor.Red); // TODO Logging
            }
            return (GetApiResponseStatus(), _responseText);
        }

        private async Task<HttpResponseMessage?> SendApiRequset(string authHeader, HttpMethod method, string? apiUrl, JsonContent? content)
        {
            var request = new HttpRequestMessage(method, apiUrl);
            request.Headers.Add("Accept", AppConfig.AcceptHeader);
            request.Headers.Add("Authorization", authHeader);
            
            if (!method.Equals(HttpMethod.Get) && content is not null)
                request.Content = content;
            Program.ColoredPrint(request.ToString()); // TODO Logging
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        private async Task GetApiResponse()
        {
            if (_httpResponse is null || !_httpResponse.IsSuccessStatusCode)
            {
                LogHttpResponseError();
                return;
            }
            Program.ColoredPrint(_httpResponse.ToString()); // TODO Logging
            var jsonResponse = await _httpResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonResponse))
                return;
            JObject jObject = JObject.Parse(jsonResponse);
            _responseText = jObject["value"]?.ToString();
        }
        private ApiResponseStatus GetApiResponseStatus()
        {
            if (_httpResponse is not null)
            {
                if (_httpResponse.IsSuccessStatusCode)
                    return ApiResponseStatus.Success;
                if (_httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                    return ApiResponseStatus.Unauthorized;
            }
            return ApiResponseStatus.Error;
        }
        private void LogHttpResponseError()
        {
            // TODO Logging
            var sb = new StringBuilder();
            sb.AppendLine($"{nameof(_httpResponse)} error at {DateTime.Now}");
            if (_httpResponse is not null)
                sb.AppendLine(_httpResponse.ToString());
            else
                sb.AppendLine($"{nameof(_httpResponse)} is null");
            Program.ColoredPrint(sb.ToString(), ConsoleColor.Red);
        }
    }
}
