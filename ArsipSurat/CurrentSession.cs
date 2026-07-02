namespace ArsipSurat
{
    public static class CurrentSession
    {
        public static int UserId { get; private set; }
        public static string Username { get; private set; }
        public static string Email { get; private set; }
        public static bool IsLoggedIn { get; private set; }

        public static void SetUser(int id, string username, string email)
        {
            UserId = id;
            Username = username;
            Email = email;
            IsLoggedIn = true;
        }

        public static void Clear()
        {
            UserId = 0;
            Username = null;
            Email = null;
            IsLoggedIn = false;
        }
    }
}
