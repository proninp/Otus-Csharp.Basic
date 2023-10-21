using MTLServiceBot.Assistants;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace MTLServiceBot.API
{
    internal class NavAPI
    {
        private static readonly HttpClient _httpClient = new();

        public NavAPI()
        {
        }

        public async Task<ApiResponse> SendApiRequsetAsync(ApiRequest apiRequest)
        {
            var apiResponseStatus = ApiResponseStatus.Error;
            var responseText = string.Empty;
            try
            {
                using (var request = new HttpRequestMessage(apiRequest.Method, apiRequest.Url))
                {
                    AddRequestHeaders(request, apiRequest.AuthHeader);
                    GetContent(request, apiRequest);

                    AssistLog.ColoredPrint(GetRequestLogInfo(request), LogStatus.Attention); // TODO Logging

                    using (var response = await _httpClient.SendAsync(request))
                    {
                        responseText = await GetHttpApiResponseAsync(response);
                        apiResponseStatus = GetApiResponseStatus(response);
                    }
                }
            }
            catch (Exception ex)
            {
                AssistLog.ColoredPrint(ex.ToString(), LogStatus.Error); // TODO Logging
            }
            var apiResponse = new ApiResponse(apiResponseStatus, responseText);
            return (apiResponse);
        }

        private void GetContent(HttpRequestMessage request, ApiRequest apiRequest)
        {
            if (apiRequest.Method.Equals(HttpMethod.Get))
                return;

            if (apiRequest.HttpContent is not null)
            {
                request.Content = apiRequest.HttpContent;
                return;
            }
            if (apiRequest.ContentStream is null)
                return;

            var content = new StreamContent(apiRequest.ContentStream);
            content.Headers.Add("Content-Type", "application/json; charset=utf-8");
            request.Content = content;
        }

        private async Task<string> GetHttpApiResponseAsync(HttpResponseMessage? httpResponse)
        {
            if (httpResponse is null || !httpResponse.IsSuccessStatusCode)
            {
                LogHttpResponseError(httpResponse);
                return string.Empty;
            }
            AssistLog.ColoredPrint(GetResponseLogInfo(httpResponse)); // TODO Logging
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
            var sb = new StringBuilder();
            sb.AppendLine($"{nameof(httpResponse)} ошибка в {DateTime.Now}");
            if (httpResponse is not null)
                sb.AppendLine(httpResponse.ToString());
            else
                sb.AppendLine($"{nameof(httpResponse)} = null");
            AssistLog.ColoredPrint(sb.ToString(), LogStatus.Error);
        }

        private void AddRequestHeaders(HttpRequestMessage request, string authHeader)
        {
            request.Headers.Add(AppConfig.AcceptHeaderName, AppConfig.AcceptHeaderValue);
            request.Headers.Add(AppConfig.AuthHeaderName, authHeader);
        }

        private string GetRequestLogInfo(HttpRequestMessage httpRequest)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(httpRequest.Method)}: {httpRequest.Method}, ");
            sb.Append($"{nameof(httpRequest.RequestUri)}: '/{httpRequest.RequestUri?.Segments.LastOrDefault()}', ");
            sb.Append($"{nameof(httpRequest.Version)}: '{httpRequest.Version}'");
            return sb.ToString();
        }
        
        private string GetResponseLogInfo(HttpResponseMessage httpResponse)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(httpResponse.StatusCode)}: {httpResponse.StatusCode}, ");
            sb.Append($"{nameof(httpResponse.ReasonPhrase)}: '{httpResponse.ReasonPhrase}', ");
            sb.Append($"{nameof(httpResponse.Version)}: '{httpResponse.Version}'");
            return sb.ToString();
        }
    }
}