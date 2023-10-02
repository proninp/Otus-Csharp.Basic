namespace MTLServiceBot.Assistants
{
    public class TextConsts
    {
        public const string StartCommandName = "/start";
        public const string StartCommandDescription = "Запускает использование бота";

        public const string StopCommandName = "/stop";
        public const string StopCommandDescription = "Завершает использования бота";
        public const string StopCommandMsg = "До свидания, {0}! Позднее, мы очистим историю взаимодействий с ботом.";

        public const string LoginCommandName = "/login";
        public const string LoginCommandDescription = "Инициирует процесс авторизации";
        public const string EnterLogin = "Введите имя пользователя";
        public const string EnterPassword = "Введите пароль";
        public const string LoginEmptyError = "Имя пользователя не может быть пустым!";
        public const string PasswordEmptyError = "Пароль не может быть пустым!";
        public const string LoginSuccessMsg = "Добро пожаловать, {0}! Вы успешно авторизованы!";

        public const string LogoutCommandName = "/logout";
        public const string LogoutCommandDescription = "Завершает сессию пользователя";

        public const string ServiceTasksCommandName = "/servicetasks";
        public const string ServiceCommandDescription = "Список сервисных запросов";

        public const string HelpCommandName = "/help";
        public const string HelpCommandDescription = "Инструкции по использованию бота";

        public const string UnknownCommandName = "/unknown";
        public const string UnknownCommandDescription = "Оповещает о неизвестной команде";
        public const string UnknownCommand = $"Команда не распознана.\nДля получения списка команд введите {HelpCommandName}";

        public const string ReceivedUpdateFailed = "Полученное обновление нераспознано";
        public const string ReceivedUpdateTypeUnknownLog = "Получен запрос с типом {0} для которого не предусмотрено обработчика";
        public const string ReceivedUpdateFromUnknown = "Нераспознан пользователь в сообщении {0}";
        public const string ReceivedUpdateTypeDataUnknown = "Нераспознан объект данных сообщения {0}";
        public const string MessageTypeError = $"Данный тип сообщений не поддерживается, для получения информации о работе бота воспользуйтесь командой {HelpCommandName}";
        public const string NewUpdateLog = "Получена команда c типом [{0}] в чате [{1}] от: [{2}]. Текст: [{3}]";
        public const string CommandExceptionLogTemplate = "Команда {0}, исключение: {1}";

        public const string LoginAlreadyAuthorized = "Вы уже авторизованы, как {0}.";
        public const string AuthorizationRequired = "Для выполнения команды {0} требуется авторизация.";

        public const string LogoutUnathorized = "Вы не авторизованы в системе, выполнение команды невозможно";
        public const string LogoutSuccess = "{0}, ваша сессия успешно завершена";

        public const string ServerConnectionError = "Ошибка соединения с сервером, выполните запрос позднее";
        public const string UnauthorizedError = $"Ошибка выполнения запроса - некорректные данные для авторизации";
        public const string DeserializeJsonError = "Ошибка преобразования результатов выполения запроса, выполните запрос позденне";

        public const string ServiceTasksListEmpty = "В системе не найдено доступных сервисных запросов";
        public const string SingleTaskNumberFormatSeparator = "; ";
        public const string ServiceTasksWorkflowIncorrectFormat = "Некорректный формат номера сервисной заявки.\n\n" +
            "Для продолжения работы введите номер сервисной заявки в форматe:\n" +
            $"<b>[Номер запроса{SingleTaskNumberFormatSeparator}Номер задачи]</b>.\n\n" +
            "Или выберите другую команду из списка меню.";
        public const string ServiceTasksWorkflowNotFound = "Не найдена сервисная заявка с номером:\n<b>{0}{2}{1}</b>\n" +
            "Для продолжения работы введите номер другой сервисной заявки.\n" +
            "Или выберите другую команду из списка меню.";

        public const string ChooseServiceRequestBtnText = "Выберите сервисную заявку из списка";
    }
}
