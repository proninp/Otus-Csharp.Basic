using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using MTLServiceBot.SQL;
using MTLServiceBot.Users;

namespace MTLServiceBot.API
{
    sealed class ServiceAPI
    {
        private static ServiceAPI? _instance;
        private static readonly object _instanceLock = new object();
        private readonly NavAPI _api;
        private readonly string _mailApiUrl;
        private readonly string _authApiUrl;
        private readonly string _serviceTasksApiUrl;
        private readonly string _serviceTaskApiUrl;
        private readonly string _setTaskStatusApiUrl;
        private readonly string _addЕпFileInfoApiUrl;
        private readonly string _getTaskFilesListUrl;
        private readonly string _addCommentApiUrl;

        private ServiceAPI()
        {
            _api = new();
            _mailApiUrl = ConfigRepository.GetApiUrl();
            _authApiUrl = $"{_mailApiUrl}/GetST";
            _serviceTasksApiUrl = $"{_mailApiUrl}/ServiceEngineerRequests";
            _serviceTaskApiUrl = $"{_mailApiUrl}/ServiceEngineerRequestsAll" + "?$filter=Request_No eq '{0}' and Task_No eq '{1}'";
            _setTaskStatusApiUrl = $"{_mailApiUrl}/SetStatus";
            _addЕпFileInfoApiUrl = $"{_mailApiUrl}/AddTelegramFile";
            _getTaskFilesListUrl = $"{_mailApiUrl}/ServiceFilesList";
            _addCommentApiUrl = $"{_mailApiUrl}/AddRequestTaskComment";
        }

        public static ServiceAPI GetInstance()
        {
            if (_instance is null)
            {
                lock (_instanceLock)
                {
                    if (_instance is null)
                        _instance = new ServiceAPI();
                }
            }
            return _instance;
        }

        public async Task<ApiResponse> Authorize(Session session)
        {
            ApiResponse authResponse;
            var apiResponse = await _api.SendServiceApiRequest(
                session.User.GetAuthUserPasswordValue(),
                HttpMethod.Post,
                _authApiUrl);

            if (apiResponse.status == ApiResponseStatus.Success && !string.IsNullOrEmpty(apiResponse.responseText))
            {
                session.SetSessionAuthorization(apiResponse.responseText);
                authResponse = new ApiResponse(apiResponse.status,
                    apiResponse.responseText,
                    string.Format(TextConsts.LoginSuccessMsg, session.User.Name));
            }
            else
                authResponse = new ApiResponse(apiResponse.status, apiResponse.responseText);
            return authResponse;
        }

        public async Task<ApiResponse> GetServiceTasks(Session session, string requestNo = "", string taskNo = "")
        {
            if (string.IsNullOrEmpty(requestNo) || string.IsNullOrEmpty(taskNo))
            {
                return await SendServiceRequest(_api.SendServiceApiRequest, session, HttpMethod.Get, _serviceTasksApiUrl);
            }
                
            return await SendServiceRequest(_api.SendServiceApiRequest, session, HttpMethod.Get, string.Format(_serviceTaskApiUrl, requestNo, taskNo));
        }

        public async Task<ApiResponse> ChangeServiceTaskStatus(Session session, ServiceTask task)
            => await SendServiceRequest(_api.SendServiceApiRequest, session, HttpMethod.Post,
                _setTaskStatusApiUrl, task.GetNewStatusContent());

        public async Task<ApiResponse> AddNewFileToServiceTask(Session session, ServiceTask task, string filename, string filePath) =>
            await SendServiceRequest(_api.SendServiceApiRequest, session, HttpMethod.Post, _addЕпFileInfoApiUrl,
                task.GetNewFileInfoContent(filename, filePath));

        private async Task<ApiResponse> SendServiceRequest(
            Func<string, HttpMethod, string, HttpContent?, Task<(ApiResponseStatus status, string responseText)>> request,
            Session session,
            HttpMethod httpMethod,
            string apiUrl,
            HttpContent? content = null)
        {
            ApiResponse authResponse;
            // Если после перезапуска приложения для сохраненной сессии еще не был получен новый токен
            if (string.IsNullOrEmpty(session.User.AuthToken))
            {
                authResponse = await Authorize(session);
                if (!authResponse.IsSuccess)
                    return authResponse;
            }
            var apiResponse = await request(session.User.GetAuthByTokenValue(), httpMethod, apiUrl, content);
            if (apiResponse.status == ApiResponseStatus.Unauthorized) // Обновляем токен, если истек срок действия
            {
                authResponse = await Authorize(session);
                if (!authResponse.IsSuccess)
                    return authResponse;
                // Если повторная авторизация прошла успешно, то заново выполняем запрос списка сервисных обращений
                apiResponse = await request(session.User.GetAuthByTokenValue(), httpMethod, apiUrl, content);
            }
            return new ApiResponse(apiResponse.status, apiResponse.responseText);
        }
    }
}