using Microsoft.IdentityModel.Tokens;

namespace bl.dto
{
    public class Users
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid RegionsID { get; set; }
        public Guid ProvincesID { get; set; }
        public Guid CityID { get; set; }
        public string Role { get; set; } = "User"; /// Default 

        public string Validate()
        {
            // Check if ManufactureName is null or empty
            if (string.IsNullOrEmpty(Firstname)) return "Username is empty or null";

            // Check if Specification is null or empty
            if (string.IsNullOrEmpty(Lastname)) return "Lastname is empty or null";

            // Check if Age is zero
            if (Age == 0) return "Age is empty or null";

            // Check if Username is null or empty
            if (string.IsNullOrEmpty(Username)) return "Lastname is empty or null";

            // Check if Password is null or empty
            if (string.IsNullOrEmpty(Password)) return "Password is empty or null";

            // Check if Leaving is null or empty
            if(RegionsID == Guid.Empty) return "Regions is empty or null";
            if(ProvincesID == Guid.Empty) return "Provinces is empty or null";
            if(CityID == Guid.Empty) return "City is empty or null";


            // Return an empty string if all validations pass
            return "";
        }

        public static async Task<string> UserLogin(string username, string password)
        {

            if (string.IsNullOrEmpty(username)) return "Username is empty or null";

            if (string.IsNullOrEmpty(password)) return "Password is empty or null";

            bl.dto.Users ret = await bl.data.User.UserLogin(username, password);

            if (ret == null) return "Invalid username or password";

            return "";
        }
        public static async Task<string> UserSignUp(bl.dto.Users dto)
        {
            // Validate the dto object
            string err = dto.Validate();

            // Return the validation error if any
            if (!string.IsNullOrEmpty(err)) return err;

            int count = await bl.data.User.CheckCountUsername(dto.Username);

            if (count > 0) return "Username Has Taken";

            // Insert the user asynchronously
            string insertResult = await bl.data.User.SignUpAsync(dto);

            return insertResult;
        }
    }
}
