﻿using ServiceRequestsHandler.API;
using ServiceRequestsHandler.DTO;
using ServiceRequestsHandler.Models;

namespace ServiceRequestsHandler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var api = new ServiceAPI();

            var userTest = new User(0, 0, Environment.GetEnvironmentVariable("API_TEST_LOGIN"));
            userTest.SetAuthPasswordTest(Environment.GetEnvironmentVariable("API_TEST_PASSWORD"));

            #region Запрос на авторизацию пользователя
            var authTokenResponse = await api.SendServiceApiRequest(userTest.GetAuthUserPasswordValue(), HttpMethod.Post, Config.AuthApiUrl);
            userTest.SetAuthTokenTest(authTokenResponse.ToString());
            #endregion

            #region Отправка запроса на получение сервисных задач
            ColoredPrint("Отправка запроса на получение сервисных задач", ConsoleColor.Magenta);
            var serviceTasksApiResponse = await api.SendServiceApiRequest(userTest.GetAuthTokenValue(), HttpMethod.Get, Config.ServiceTasksApiUrl);
            Console.WriteLine(serviceTasksApiResponse);
            #endregion

            if (serviceTasksApiResponse.Status == ResponseStatus.Success)
            {
                var serviceTaskList = System.Text.Json.JsonSerializer.Deserialize<List<ServiceTask>>(serviceTasksApiResponse.ToString());
                foreach (var sTask in serviceTaskList)
                    Console.WriteLine(sTask);
                var serviceTask = serviceTaskList.First();

                #region Добавление нового комментария по сервисному запросу
                //var addCommentApiResponse = await api.SendServiceApiRequest(userTest.GetAuthTokenValue(),
                //    HttpMethod.Post, Config.AddCommentApiUrl,
                //    serviceTask.GetNewCommentContent($"Новый комментарий, добавленный в {DateTime.Now}"));
                #endregion

                #region Получение списка комментариев по сервисному запросу
                ColoredPrint("Получение списка комментариев по сервисному запросу", ConsoleColor.Magenta);
                var commentsResponse = await api.SendServiceApiRequest(userTest.GetAuthTokenValue(),
                    HttpMethod.Post, serviceTask.GetTaskCommetsUrl());
                serviceTask.AddCommentsFromJsonResponse(commentsResponse);
                #endregion

                #region Изменение статуса сервисной задачи
                var newStatusContet = serviceTask.GetNewStatusContent(ServiceTaskStatus.Execution);
                var newSetviceTaskStatus = await api.SendServiceApiRequest(
                    userTest.GetAuthTokenValue(), HttpMethod.Post, Config.SetTaskStatusApiUrl, newStatusContet);
                Console.WriteLine($"Новый статус: {newSetviceTaskStatus}");
                #endregion

                #region Получение списка файлов по задаче
                ColoredPrint("Получение списка файлов по задаче", ConsoleColor.Magenta);
                var serviceTaskFilesListResponse = await api.SendServiceApiRequest(userTest.GetAuthTokenValue(), HttpMethod.Get, serviceTask.GetServiceTaskFilesListUtl());
                Console.WriteLine(serviceTaskFilesListResponse);
                serviceTask.AddFilesInfoFromJsonResponse(serviceTaskFilesListResponse);
                #endregion

                Console.WriteLine(serviceTask);
            }
            Console.ReadLine();
        }
        public static void ColoredPrint(string text, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
    }
}