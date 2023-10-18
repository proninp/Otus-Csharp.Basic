using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using MTLServiceBot.SQL;
using MTLServiceBot.Users;

namespace MTLServiceBot.API
{
    sealed class ServiceAPI
    {
        private static string _mailApiUrl = ConfigRepository.GetApiUrl();
        private static string _authApiUrl = $"{_mailApiUrl}/GetST";
        private static string _serviceTasksApiUrl = $"{_mailApiUrl}/ServiceEngineerRequests";
        private static string _serviceTaskApiUrl = _mailApiUrl + "/ServiceEngineerRequestsAll?$filter=Request_No eq '{0}' and Task_No eq '{1}'";
        private static string _setTaskStatusApiUrl = $"{_mailApiUrl}/SetStatus";
        private static string _addFileApiUrl = $"{_mailApiUrl}/AddServiceFile";
        private static string _addNetworkFileApiUrl = $"{_mailApiUrl}/AddTelegramFile";
        private static string _getTaskFilesListUrl = $"{_mailApiUrl}/ServiceFilesList";
        private static string _addCommentApiUrl = $"{_mailApiUrl}/AddRequestTaskComment";
        
        private readonly NavAPI _api;
        private readonly Session _session;

        public ServiceAPI(Session session)
        {
            _api = new();
            _session = session;
        }

        public async Task<ApiResponse> AuthorizeAsync()
        {
            var authApiRequest = new ApiRequest(_authApiUrl, HttpMethod.Post, _session.User.GetUserPassAuthHeader());
            var authApiResponse = await _api.SendApiRequsetAsync(authApiRequest);

            if (authApiResponse.IsSuccess && !string.IsNullOrEmpty(authApiResponse.ResponseText))
            {
                _session.SetSessionAuthorization(authApiResponse.ResponseText);
                authApiResponse.Message = string.Format(TextConsts.LoginSuccessMsg, _session.User.Name);
            }
            return authApiResponse;
        }

        public async Task<ApiResponse> GetServiceTasksAsync(string? requestNo = null, string? taskNo = null)
        {
            ApiRequest apiRequest;
            if (string.IsNullOrEmpty(requestNo) || string.IsNullOrEmpty(taskNo))
            {
                apiRequest = new ApiRequest(_serviceTasksApiUrl, HttpMethod.Get, _session.User.GetTokenAuthHeader());
                return await SendServiceRequestAsync(apiRequest);
            }

            apiRequest = new ApiRequest(string.Format(_serviceTaskApiUrl, requestNo, taskNo),
                HttpMethod.Get, _session.User.GetTokenAuthHeader());

            return await SendServiceRequestAsync(apiRequest);
        }

        public async Task<ApiResponse> ChangeServiceTaskStatusAsync(ServiceTask task)
        {
            var apiRequest = new ApiRequest(_setTaskStatusApiUrl, HttpMethod.Post,
                _session.User.GetTokenAuthHeader(), task.GetNewStatusContent());

            return await SendServiceRequestAsync(apiRequest);
        }

        public async Task<ApiResponse> AddNewFileToServiceTaskAsync(ServiceTask task, string filePath, string filename, string fileDescription = "")
        {
            var apiRequest = new ApiRequest(_addNetworkFileApiUrl, HttpMethod.Post,
                _session.User.GetTokenAuthHeader(), task.GetNewNetworkFileContent(filename, filePath, fileDescription));

            return await SendServiceRequestAsync(apiRequest);
        }

        public async Task<ApiResponse> AddNewFileToServiceTaskAsync(ServiceTask task, Stream fileStream, string filename, string fileDescription = "")
        {
            using var fileContentStream = task.GetNewFileStreamContent(fileStream, filename, fileDescription);
            var apiRequest = new ApiRequest(_addFileApiUrl, HttpMethod.Post,
                _session.User.GetTokenAuthHeader(), fileContentStream);

            return await SendServiceRequestAsync(apiRequest);
        }

        private async Task<ApiResponse> SendServiceRequestAsync(ApiRequest apiRequest)
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