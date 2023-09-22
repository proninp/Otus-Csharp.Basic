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
        private readonly string _setTaskStatusApiUrl;
        private readonly string _setTaskFilesListUrl;
        private readonly string _addCommentApiUrl;

        private ServiceAPI()
        {
            _api = new();
            _mailApiUrl = ConfigRepository.GetApiUrl();
            _authApiUrl = $"{_mailApiUrl}/GetST";
            _serviceTasksApiUrl = $"{_mailApiUrl}/ServiceEngineerRequests";
            _setTaskStatusApiUrl = $"{_mailApiUrl}/SetStatus";
            _setTaskFilesListUrl = $"{_mailApiUrl}/ServiceFilesList";
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

        public async Task<ApiResponse> Authorize(Session session) =>
            await _api.SendServiceApiRequest(session.User.GetAuthUserPasswordValue(), HttpMethod.Post, _authApiUrl);
    }
}
