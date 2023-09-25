namespace MTLServiceBot.Bot
{
    public class CaptionConstants
    {
        public const string StartCaption = "/start";
        public const string StartDescription = "Запускает использование бота";

        public const string StopCaption = "/stop";
        public const string StopDescription = "Завершает использования бота";

        public const string LoginCaption = "/login";
        public const string LoginDescription = "Инициирует процесс авторизации";

        public const string LogoutCaption = "/logout";
        public const string LogoutDescription = "Завершает сессию пользователя";

        public const string ServiceTasksCaption = "/servicetasks";
        public const string ServiceDescription = "Список сервисных запросов";

        public const string HelpCaption = "/help";
        public const string HelpDescription = "Инструкции по использованию бота";

        public const string UnknownCaption = "/unknown";
        public const string UnknownDescription = "/Оповещает о неизвестной команде";

        public const string ServerConnectionError = "Ошибка соединения с сервером, выполните запрос позднее";
        public const string UnauthorizedError = $"Ошибка авторизации при выполнении запроса, выполните команду {LoginCaption} и попробуйте снова";
        public const string DeserializeJsonError = "Ошибка преобразования результатов выполения запроса, выполните запрос позденне";

        public const string ServiceTasksListEmpty = "В системе не найдено доступных для вывода сервисных запросов";
        


    }
}
