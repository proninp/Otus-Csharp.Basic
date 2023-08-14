using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace ServiceRequestsHandler.API
{
    public class ServiceAPI
    {
        private static readonly HttpClient HttpClient = new();

        public async Task<ApiResponse> SendServiceApiRequest(string authHeader, HttpMethod method, string? apiUrl, JsonContent? content = null)
        {
            ApiResponse apiResponse = new();
            try
            {
                string requstContentTester;
                if (content != null)
                    requstContentTester = content.Value.ToString();
                HttpResponseMessage? httpResponseMessage = await SendApiRequset(authHeader, method, apiUrl, content);
                var task = apiResponse.GetApiResponse(httpResponseMessage);
            }
            catch (Exception ex)
            {
                apiResponse.SetDefaultException();
                Program.ColoredPrint(ex.ToString(), ConsoleColor.Red); // TODO Logging
            }
            return apiResponse;
        }

        private async Task<HttpResponseMessage?> SendApiRequset(string authHeader, HttpMethod method, string? apiUrl, JsonContent? content)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(method, apiUrl);
                request.Headers.Add("Accept", Config.AcceptHeader);
                request.Headers.Add("Authorization", authHeader);
                if (!method.Equals(HttpMethod.Get) && content is not null)
                    request.Content = content;
                var response = await HttpClient.SendAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); // TODO Logging
            }
            return null;
        }

    }
}
