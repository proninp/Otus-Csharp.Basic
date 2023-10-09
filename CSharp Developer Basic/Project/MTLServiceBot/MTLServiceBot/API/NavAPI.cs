using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace MTLServiceBot.API
{
    internal class NavAPI
    {
        private static readonly HttpClient _httpClient = new();

        public NavAPI() 
        {
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
            var responseText = string.Empty;
            var responseStatus = ApiResponseStatus.Error;

            try
            {
                (responseStatus, responseText) = await SendApiRequset(authHeader, method, apiUrl, content);
            }
            catch (Exception ex)
            {
                Program.ColoredPrint(ex.ToString(), ConsoleColor.Red); // TODO Logging
            }
            return (responseStatus, responseText);
        }

        private async Task<(ApiResponseStatus, string)> SendApiRequset(string authHeader, HttpMethod method, string? apiUrl, JsonContent? content)
        {
            using var request = new HttpRequestMessage(method, apiUrl);
            request.Headers.Add("Accept", AppConfig.AcceptHeader);
            request.Headers.Add("Authorization", authHeader);
            
            if (!method.Equals(HttpMethod.Get) && content is not null)
                request.Content = content;
            Program.ColoredPrint(request.ToString()); // TODO Logging
            var responseText = string.Empty;
            var apiResponseStatus = ApiResponseStatus.Error;
            using (var response = _httpClient.Send(request))
            {
                responseText = await GetApiResponse(response);
                apiResponseStatus = GetApiResponseStatus(response);
                response.Dispose();
            }
            return (apiResponseStatus, responseText);
        }

        private async Task<string> GetApiResponse(HttpResponseMessage? httpResponse)
        {
            if (httpResponse is null || !httpResponse.IsSuccessStatusCode)
            {
                LogHttpResponseError(httpResponse);
                return string.Empty;
            }
            
            Program.ColoredPrint(httpResponse.ToString()); // TODO Logging
            using var responseContent = httpResponse.Content;
            var jsonResponse = await responseContent.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonResponse))
                return string.Empty;
            
            JObject jObject = JObject.Parse(jsonResponse);
            return jObject["value"]?.ToString() ?? "";
        }

        private ApiResponseStatus GetApiResponseStatus(HttpResponseMessage? response)
        {
            if (response is not null)
            {
                if (response.IsSuccessStatusCode)
                    return ApiResponseStatus.Success;
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return ApiResponseStatus.Unauthorized;
            }
            return ApiResponseStatus.Error;
        }

        private void LogHttpResponseError(HttpResponseMessage? httpResponse)
        {
            // TODO Logging
            var sb = new StringBuilder();
            sb.AppendLine($"{nameof(httpResponse)} error at {DateTime.Now}");
            if (httpResponse is not null)
                sb.AppendLine(httpResponse.ToString());
            else
                sb.AppendLine($"{nameof(httpResponse)} is null");
            Program.ColoredPrint(sb.ToString(), ConsoleColor.Red);
        }
    }
}
