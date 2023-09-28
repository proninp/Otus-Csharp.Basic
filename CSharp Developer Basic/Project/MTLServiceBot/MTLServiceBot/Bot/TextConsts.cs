namespace MTLServiceBot.Bot
{
    public class TextConsts
    {
        public const string StartCommandName = "/start";
        public const string StartCommandDescription = "Запускает использование бота";

        public const string StopCommandName = "/stop";
        public const string StopCommandDescription = "Завершает использования бота";

        public const string LoginCommandName = "/login";
        public const string LoginCommandDescription = "Инициирует процесс авторизации";

        public const string LogoutCommandName = "/logout";
        public const string LogoutCommandDescription = "Завершает сессию пользователя";

        public const string ServiceTasksCommandName = "/servicetasks";
        public const string ServiceCommandDescription = "Список сервисных запросов";

        public const string HelpCommandName = "/help";
        public const string HelpCommandDescription = "Инструкции по использованию бота";

        public const string UnknownCommandName = "/unknown";
        public const string UnknownCommandDescription = "Оповещает о неизвестной команде";
        public const string UnknownCommandMsg = $"Команда не распознана.\nДля получения списка команд введите {HelpCommandName}";

        public const string LoginAlreadyAuthorizedMsg = "Вы уже авторизованы, как {0}.";

        public const string LogoutUnathorizedMsg = "Вы не авторизованы в системе, выполнение команды невозможно";
        public const string LogoutSuccessMsg = "{0}, ваша сессия успешно завершена";

        public const string ServerConnectionError = "Ошибка соединения с сервером, выполните запрос позднее";
        public const string UnauthorizedError = $"Ошибка выполнения запроса - некорректные данные для авторизации";
        public const string DeserializeJsonError = "Ошибка преобразования результатов выполения запроса, выполните запрос позденне";

        public const string ServiceTasksListEmpty = "В системе не найдено доступных для вывода сервисных запросов";
        public const string SingleTaskNumberFormatSeparator = "; ";
        public const string ServiceTasksWorkflowIncorrectFormat = "Некорректный формат номера сервисной заявки.\n\n" +
            "Для продолжения работы введите номер сервисной заявки в форматe:\n" +
            $"**[Запрос Но.{SingleTaskNumberFormatSeparator}Задача Но.]**.\n\n" +
            "Или выберите другую команду из списка меню.";
        public const string ServiceTasksWorkflowNotFound = "Не найдена сервисная заявка с номером:\n**{0}{2}{1}**\n" +
            "Для продолжения работы введите номер другой сервисной заявки.\n" +
            "Или выберите другую команду из списка меню.";

        public const string ChooseServiceRequestBtnText = "Выберите сервисную заявку из списка";
    }
}
