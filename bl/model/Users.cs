namespace bl.model
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }


        public static async Task<bl.model.Users> CheackUserHave(string Username)
        {
            bl.model.Users users = await bl.data.User.CheackUserHave(Username);

            return users;
        }




    }
}
