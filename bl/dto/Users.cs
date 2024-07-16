namespace bl.dto
{
    public class Users
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid LeavingID { get; set; }
        public string Role { get; set; } = "User"; /// Default 



        public static async Task<string> UserLogin(string username, string password)
        {

    

            if (string.IsNullOrEmpty(username)) return "Username is empty or null";

            if (string.IsNullOrEmpty(password)) return "Password is empty or null";

            bl.dto.Users ret = await bl.data.User.UserLogin(username, password);

            if (ret == null) return "Invalid username or password";

            return "";
        }

    }
}
