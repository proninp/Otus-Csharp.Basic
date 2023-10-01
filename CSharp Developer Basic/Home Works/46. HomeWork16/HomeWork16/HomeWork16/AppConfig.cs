namespace HomeWork16
{
    public static class AppConfig
    {
        public static readonly string DbConnectionString = Environment.GetEnvironmentVariable("OTUS_HW16_CONNECTION") ??
            "Host=localhost;Username=postgres;Password=password;Database=Shop";
    }
}
