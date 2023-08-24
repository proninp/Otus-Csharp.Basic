namespace MTLServiceBot.Users
{
    internal class UserSession
    {
        private static Dictionary<long, UserSession> userSessions = new Dictionary<long, UserSession>();
        public User User { get; set; }
        public long Id { get; set; }
        public long ChatId { get; set; }
        public AuthenticationStep AuthenticationStep { get; set; }
        public bool IsAuthenticated { get; set; }

        public static bool CheckAuthAsync(long id)
        {
            return userSessions.ContainsKey(id) && userSessions[id].IsAuthenticated;
        }
    }
}
