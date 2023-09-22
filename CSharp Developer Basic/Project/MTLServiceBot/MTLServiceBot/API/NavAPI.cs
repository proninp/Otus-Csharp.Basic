using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;

namespace MTLServiceBot.API
{
    internal class NavAPI
    {
        private static HttpClient? _httpClient;
        public ApiResponseStatus ResponseStatus
        {
            get
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
        }
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
        public async Task<ApiResponse> SendServiceApiRequest(string authHeader, HttpMethod method, string? apiUrl, JsonContent? content = null)
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
            return new ApiResponse(ResponseStatus, _responseText);
        }

        private async Task<HttpResponseMessage?> SendApiRequset(string authHeader, HttpMethod method, string? apiUrl, JsonContent? content)
        {
            var request = new HttpRequestMessage(method, apiUrl);
            request.Headers.Add("Accept", AppConfig.AcceptHeader);
            request.Headers.Add("Authorization", authHeader);
            
            if (!method.Equals(HttpMethod.Get) && content is not null)
                request.Content = content;
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        private async Task GetApiResponse()
        {
            if (_httpResponse is null || !_httpResponse.IsSuccessStatusCode)
                return;
            
            var jsonResponse = await _httpResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonResponse))
                return;
            JObject jObject = JObject.Parse(jsonResponse);
            _responseText = jObject["value"]?.ToString();
        }
    }
}
