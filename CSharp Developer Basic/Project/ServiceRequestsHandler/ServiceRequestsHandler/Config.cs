using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestsHandler
{
    public static class Config
    {
        public const string AcceptHeader = "application/json";
        private static readonly string? mainApiUrl = Environment.GetEnvironmentVariable("MTL_MAIN_API_URL");
        public static readonly string? AuthApiUrl = Environment.GetEnvironmentVariable("MTL_AUTH_API_URL");
        public static readonly string? ServiceRequestsListApiUrl = $"{mainApiUrl}/ServiceEngineerRequestsAll";
        public static readonly string? BotToken = Environment.GetEnvironmentVariable("MTL_TG_BOT_TOKEN");
    }
}
