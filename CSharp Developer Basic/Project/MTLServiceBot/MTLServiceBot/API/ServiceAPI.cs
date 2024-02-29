using MTLServiceBot.API.Entities;
using MTLServiceBot.Users;

namespace MTLServiceBot.API
{
    sealed class ServiceAPI
    {
        private readonly NavAPI _api;
        private readonly Session _session;

        public ServiceAPI(Session session)
        {
            _api = new();
            _session = session;
        }

        public async Task<ApiResponse> AuthorizeAsync()
        {
            var authApiRequest = new ApiRequest(AppConfig.Instance.AuthApiUrl, HttpMethod.Post, _session.User.GetUserPassAuthHeader());
            var authApiResponse = await _api.SendApiRequsetAsync(authApiRequest);

            if (authApiResponse.IsSuccess && !string.IsNullOrEmpty(authApiResponse.ResponseText))
            {
                _session.SetSessionAuthorization(authApiResponse.ResponseText);
                authApiResponse.Message = string.Format(AppConfig.Instance.LoginSuccessMsg, _session.User.Name);
            }
            return authApiResponse;
        }

        public async Task<ApiResponse> GetServiceTasksAsync(string? requestNo = null, string? taskNo = null)
        {
            ApiRequest apiRequest;
            if (string.IsNullOrEmpty(requestNo) || string.IsNullOrEmpty(taskNo))
            {
                apiRequest = new ApiRequest(AppConfig.Instance.ServiceTasksApiUrl, HttpMethod.Get, _session.User.GetTokenAuthHeader());
                return await SendNavRequestAsync(apiRequest);
            }

            apiRequest = new ApiRequest(string.Format(AppConfig.Instance.ServiceTaskApiUrl, requestNo, taskNo),
                HttpMethod.Get, _session.User.GetTokenAuthHeader());

            return await SendNavRequestAsync(apiRequest);
        }

        public async Task<ApiResponse> ChangeServiceTaskStatusAsync(ServiceTask task)
        {
            var apiRequest = new ApiRequest(AppConfig.Instance.SetTaskStatusApiUrl, HttpMethod.Post,
                _session.User.GetTokenAuthHeader(), task.GetNewStatusContent());

            return await SendNavRequestAsync(apiRequest);
        }

        public async Task<ApiResponse> AddNewFileToServiceTaskAsync(ServiceTask task, string filePath, string filename, string fileDescription = "")
        {
            var apiRequest = new ApiRequest(AppConfig.Instance.AddNetworkFileApiUrl, HttpMethod.Post,
                _session.User.GetTokenAuthHeader(), task.GetNewNetworkFileContent(filename, filePath, fileDescription));

            return await SendNavRequestAsync(apiRequest);
        }

        public async Task<ApiResponse> AddNewFileToServiceTaskAsync(ServiceTask task, Stream fileStream, string filename, string fileDescription = "")
        {
            using var fileContentStream = task.GetNewFileStreamContent(fileStream, filename, fileDescription);
            var apiRequest = new ApiRequest(AppConfig.Instance.AddFileApiUrl, HttpMethod.Post,
                _session.User.GetTokenAuthHeader(), fileContentStream);

            return await SendNavRequestAsync(apiRequest);
        }

        public async Task<ApiResponse> GetIntegrisPassAsync(string challenge)
        {
            var apiRequest = new ApiRequest(AppConfig.Instance.OtpgenApiUrl, HttpMethod.Post, _session.User.GetTokenAuthHeader(), new StringContent(challenge));
            return await SendNavRequestAsync(apiRequest);
        }

        private async Task<ApiResponse> SendNavRequestAsync(ApiRequest apiRequest)
        {
            ApiResponse authResponse;
            // Если после перезапуска приложения для сохраненной сессии еще не был получен новый токен
            if (string.IsNullOrEmpty(_session.User.AuthToken))
            {
                authResponse = await AuthorizeAsync();
                if (!authResponse.IsSuccess)
                    return authResponse;
                apiRequest.AuthHeader = _session.User.GetTokenAuthHeader();
            }

            var apiResponse = await _api.SendApiRequsetAsync(apiRequest);
            if (apiResponse.Status == ApiResponseStatus.Unauthorized) // Обновляем токен, если истек срок действия
            {
                authResponse = await AuthorizeAsync();
                if (!authResponse.IsSuccess)
                    return authResponse;
                apiRequest.AuthHeader = _session.User.GetTokenAuthHeader();

                // Если повторная авторизация прошла успешно, то заново выполняем запрос списка сервисных обращений
                apiResponse = await _api.SendApiRequsetAsync(apiRequest);
            }
            return apiResponse;
        }
    }
}