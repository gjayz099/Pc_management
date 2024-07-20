namespace bl.data
{
    public class User
    {

        public static async Task<bl.dto.Users> UserLogin(string username, string password)
        {
            string sqlSelectUser = $@"
                SELECT
	                Firstname
	                ,Lastname
	                ,Username
	                ,Password
                    ,Role
                FROM {bl.refs.Databse_DB}.dbo.pcpms_user
                where Username = @Username and Password = @Password";

            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Username", Value = username},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Password", Value = password}
            };


            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlSelectUser, par, x => new bl.dto.Users
            {


           
                Firstname = (x[0] == DBNull.Value) ? string.Empty : (string)x[0],
                Lastname = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Username = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                Password = (x[3] == DBNull.Value) ? string.Empty : (string)x[3],
                Role = (x[4] == DBNull.Value) ? string.Empty : (string)x[4],
            });


            if (ret.status && ret.data != null && ret.data.Count > 0) return ret.data[0];


            return null;

        }


        public static async Task<bl.model.Users> CheackUserHave(string username)
        {
            string sqlSelectUser = $@"
                SELECT
	                Firstname
	                ,Lastname
	                ,Username
	                ,Password
                    ,Role
                FROM {bl.refs.Databse_DB}.dbo.pcpms_user
                where Username = @Username";

            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Username", Value = username},
            
            };


            var ret = await bl.DBaccess.RawSqlQuerySingleAsync(sqlSelectUser, par, x => new bl.model.Users
            {



                Firstname = (x[0] == DBNull.Value) ? string.Empty : (string)x[0],
                Lastname = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Username = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                Password = (x[3] == DBNull.Value) ? string.Empty : (string)x[3],
                Role = (x[4] == DBNull.Value) ? string.Empty : (string)x[4],
            });

            return ret.data;

        }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // ComputeHash returns byte array, convert it to a string
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                // Convert byte array to a 'safe' string representation
                return Convert.ToBase64String(hashedBytes);
            }
        }


        // Method to handle user signup
        public static async Task<string> SignUpAsync(bl.dto.Users user)
        {
            string hashedPassword = HashPassword(user.Password);

            // Example SQL query to insert a new user into the database
            string query = $@"INSERT INTO
                        FROM {bl.refs.Databse_DB}.dbo.pcpms_user (
                            Id
                            ,Firstname
                            ,Lastname
                            ,Username
                            ,Password
                            ,Role)
                        VALUES (
                            NEWID()
                            ,@Firstname
                            ,@Lastname
                            ,@Username
                            ,@Password
                            ,@Role)";


            var result = await bl.DBaccess.ExecNonQueryAsync(query, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter("@Firstname", user.Firstname),
                new Microsoft.Data.SqlClient.SqlParameter("@Lastname", user.Lastname),
                new Microsoft.Data.SqlClient.SqlParameter("@Username", user.Username),
                new Microsoft.Data.SqlClient.SqlParameter("@Password", hashedPassword),
                new Microsoft.Data.SqlClient.SqlParameter("@Role", user.Role),
            });

            if (!string.IsNullOrEmpty(result.err))
            {
                return $"Error inserting user: {result.err}";
            }
            return $"Sign Up Successful for {user.Username}.";

        }
        public static async Task<int> CheckCountUsername(string username)
        {
            string query = $@"SELECT COUNT(*) FROM {bl.refs.Databse_DB}.dbo.pcpms_user WHERE Username = @Username";

            var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter { ParameterName = "@Username", Value = username }
            };

            int count = await bl.DBaccess.ExecuteScalarAsync(query, parameters);

            return count;
        }


    }
}
