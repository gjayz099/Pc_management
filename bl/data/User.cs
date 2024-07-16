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
	                ,Age
	                ,Username
	                ,Password
                    ,Role
                FROM gerald_pcpms_db.dbo.pcpms_user
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
                Age = (x[2] == DBNull.Value) ? 0 : (int)x[2],
                Username = (x[3] == DBNull.Value) ? string.Empty : (string)x[3],
                Password = (x[4] == DBNull.Value) ? string.Empty : (string)x[4],
                Role = (x[5] == DBNull.Value) ? string.Empty : (string)x[5],
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
	                ,Age
	                ,Username
	                ,Password
                    ,Role
                FROM gerald_pcpms_db.dbo.pcpms_user
                where Username = @Username";

            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Username", Value = username},
            
            };


            var ret = await bl.DBaccess.RawSqlQuerySingleAsync(sqlSelectUser, par, x => new bl.model.Users
            {



                Firstname = (x[0] == DBNull.Value) ? string.Empty : (string)x[0],
                Lastname = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Age = (x[2] == DBNull.Value) ? 0 : (int)x[2],
                Username = (x[3] == DBNull.Value) ? string.Empty : (string)x[3],
                Password = (x[4] == DBNull.Value) ? string.Empty : (string)x[4],
                Role = (x[5] == DBNull.Value) ? string.Empty : (string)x[5],
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

    }
}
