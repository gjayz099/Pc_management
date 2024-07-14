using Microsoft.Data.SqlClient;

namespace bl
{
    public class DBaccess
    {

        // Method to execute a raw SQL query asynchronously and map results to a list of entities
        public static async Task<List<T>> OldRawSqlQueryAsync<T>(string query, Func<System.Data.Common.DbDataReader, T> map)
        {
            try
            {
                DateTime start = DateTime.Now;  // Record the current time for measuring query execution duration

                // Establish a new SQL connection using the connection string retrieved from ConHelper
                using (var con = new Microsoft.Data.SqlClient.SqlConnection(ConHelper.ConnectionConfiguration()))
                {
                    // Create a new SQL command with the provided query and connection
                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, con))
                    {
                        command.CommandType = System.Data.CommandType.Text;  // Specify that the command type is text (SQL query)
                        command.CommandTimeout = 1200;  // Set command timeout to 1200 seconds (20 minutes)
                        con.Open();  // Open the database connection

                        // Execute the command asynchronously and retrieve a data reader
                        using (var result = await command.ExecuteReaderAsync())
                        {
                            var entities = new List<T>();  // Initialize a list to store mapped entities

                            // Read each row from the data reader and map it using the provided mapping function
                            while (result.Read())
                            {
                                entities.Add(map(result));  // Map the current row and add it to the entities list
                            }

                            result.Close();  // Close the data reader
                            con.Close();  // Close the database connection

                            // Log slow queries if the execution time exceeds 3 seconds
                            if ((DateTime.Now - start).TotalSeconds > 3)
                            {
                                bl.sys.writelog("sql_PCPMS_slow", (DateTime.Now - start).TotalSeconds.ToString() + "secs SLOW: " + query + "\r\n" + Environment.StackTrace, true);
                            }

                            return entities;  // Return the list of mapped entities
                        }
                    }
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException sqex)
            {
                // Log SQL exceptions including the query and exception details
                bl.sys.writelog("error_PCPMS_executequery", "Error: " + query + "\r\n" + bl.sys.ReadException(sqex) + "/" + sqex.StackTrace);
            }
            catch (Exception ex)
            {
                // Log general exceptions including the query and exception details
                bl.sys.writelog("error_PCPMS_executequery", "Error: " + query + "\r\n" + bl.sys.ReadException(ex) + "/" + ex.StackTrace);
            }

            return null;  // Return null if an exception occurs
        }

        // Method to execute a raw SQL query asynchronously and return a tuple containing status, error message, and data
        public static async Task<(bool status, string errorstr, List<T> data)> RawSqlQueryAsync<T>(string query, Func<System.Data.Common.DbDataReader, T> map)
        {
            string errormsg = "";  // Initialize an empty string to store error messages

            try
            {
                DateTime start = DateTime.Now;  // Record the current time for measuring query execution duration

                // Establish a new SQL connection using the connection string retrieved from ConHelper
                using (var sqlCn = new Microsoft.Data.SqlClient.SqlConnection(ConHelper.ConnectionConfiguration()))
                {
                    // Create a new SQL command with the provided query and connection
                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, sqlCn))
                    {
                        command.CommandType = System.Data.CommandType.Text;  // Specify that the command type is text (SQL query)
                        command.CommandTimeout = 1200;  // Set command timeout to 1200 seconds (20 minutes)
                        sqlCn.Open();  // Open the database connection

                        // Execute the command asynchronously and retrieve a data reader
                        using (var result = await command.ExecuteReaderAsync())
                        {
                            var entities = new List<T>();  // Initialize a list to store mapped entities

                            // Read each row from the data reader and map it using the provided mapping function
                            while (result.Read())
                            {
                                entities.Add(map(result));  // Map the current row and add it to the entities list
                            }

                            result.Close();  // Close the data reader
                            sqlCn.Close();  // Close the database connection

                            // Log slow queries if the execution time exceeds 3 seconds
                            if ((DateTime.Now - start).TotalSeconds > 3)
                            {
                                bl.sys.writelog("sql_PCPMS_slow", (DateTime.Now - start).TotalSeconds.ToString() + "secs SLOW: " + query + "\r\n" + Environment.StackTrace, true);
                            }

                            // Return a tuple with success status, empty error message, and the list of entities
                            return (true, "", entities);
                        }
                    }
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException sqex)
            {
                errormsg = "Error: " + sqex.Message;  // Format error message for SQL exceptions
                bl.sys.writelog("error_PCPMS_executequery", "Error: " + query + "\r\n" + bl.sys.ReadException(sqex) + "/" + sqex.StackTrace);  // Log SQL exception details
            }
            catch (Exception ex)
            {
                errormsg = "Error: " + ex.Message;  // Format error message for general exceptions
                bl.sys.writelog("error_PCPMS_executequery", "Error: " + query + "\r\n" + bl.sys.ReadException(ex) + "/" + ex.StackTrace);  // Log general exception details
            }

            // Return a tuple with failure status, the captured error message, and null data
            return (false, errormsg, null);
        }


        // Method to execute a raw SQL query asynchronously with parameters and return status, error message, and data
        public static async Task<(bool status, string errorstr, List<T> data)> RawSqlQueryAsync<T>(string query, List<Microsoft.Data.SqlClient.SqlParameter> _params, Func<System.Data.Common.DbDataReader, T> map)
        {
            string errormsg = "";  // Initialize an empty string to store error messages

            try
            {
                DateTime start = DateTime.Now;  // Record the current time for measuring query execution duration

                // Establish a new SQL connection using the connection string retrieved from ConHelper
                using (var sqlCn = new Microsoft.Data.SqlClient.SqlConnection(ConHelper.ConnectionConfiguration()))
                {
                    // Create a new SQL command with the provided query and connection
                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, sqlCn))
                    {
                        command.CommandType = System.Data.CommandType.Text;  // Specify that the command type is text (SQL query)
                        command.CommandTimeout = 1200;  // Set command timeout to 1200 seconds (20 minutes)

                        // Add parameters to the SQL command
                        foreach (var p in _params)
                        {
                            command.Parameters.Add(p);
                        }

                        sqlCn.Open();  // Open the database connection

                        // Execute the command asynchronously and retrieve a data reader
                        using (var result = await command.ExecuteReaderAsync())
                        {
                            var entities = new List<T>();  // Initialize a list to store mapped entities

                            // Read each row from the data reader and map it using the provided mapping function
                            while (result.Read())
                            {
                                entities.Add(map(result));  // Map the current row and add it to the entities list
                            }

                            result.Close();  // Close the data reader
                            sqlCn.Close();  // Close the database connection

                            // Log slow queries if the execution time exceeds 3 seconds
                            if ((DateTime.Now - start).TotalSeconds > 3)
                            {
                                sys.writelog("sql_PCPMS_slow", (DateTime.Now - start).TotalSeconds.ToString() + "secs SLOW: " + query + "\r\n" + Environment.StackTrace, true);
                            }

                            // Return a tuple with success status, empty error message, and the list of entities
                            return (true, "", entities);
                        }
                    }
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException sqex)
            {
                errormsg = "Error: " + sqex.Message;  // Format error message for SQL exceptions
                sys.writelog("error_PCPMS_executequery", "Error: " + query + "\r\n" + sys.ReadException(sqex) + "/" + sqex.StackTrace);  // Log SQL exception details
            }
            catch (Exception ex)
            {
                errormsg = "Error: " + ex.Message;  // Format error message for general exceptions
                sys.writelog("error_PCPMS_executequery", "Error: " + query + "\r\n" + sys.ReadException(ex) + "/" + ex.StackTrace);  // Log general exception details
            }

            // Return a tuple with failure status, the captured error message, and null data
            return (false, errormsg, null);
        }


        public static async Task<(bool status, string errorstr, T data)> RawSqlQuerySingleAsync<T>(string query, List<Microsoft.Data.SqlClient.SqlParameter> _params, Func<System.Data.Common.DbDataReader, T> map)
        {
            string errormsg = "";

            try
            {
                DateTime start = DateTime.Now;

                using (var sqlCn = new Microsoft.Data.SqlClient.SqlConnection(ConHelper.ConnectionConfiguration()))
                {
                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, sqlCn))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 1200;

                        foreach (var p in _params)
                        {
                            command.Parameters.Add(p);
                        }

                        sqlCn.Open();

                        using (var result = await command.ExecuteReaderAsync())
                        {
                            if (result.Read())
                            {
                                T entity = map(result);

                                result.Close();
                                sqlCn.Close();

                                if ((DateTime.Now - start).TotalSeconds > 3)
                                {
                                    sys.writelog("sql_PCPMS_slow", (DateTime.Now - start).TotalSeconds.ToString() + "secs SLOW: " + query + "\r\n" + Environment.StackTrace, true);
                                }

                                return (true, "", entity);
                            }
                            else
                            {
                                result.Close();
                                sqlCn.Close();

                                // Handle case where no data was found
                                return (false, "No data found for the query", default);
                            }
                        }
                    }
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException sqex)
            {
                errormsg = "Error: " + sqex.Message;
                sys.writelog("error_PCPMS_executequery", "Error: " + query + "\r\n" + sys.ReadException(sqex) + "/" + sqex.StackTrace);
            }
            catch (Exception ex)
            {
                errormsg = "Error: " + ex.Message;
                sys.writelog("error_PCPMS_executequery", "Error: " + query + "\r\n" + sys.ReadException(ex) + "/" + ex.StackTrace);
            }

            return (false, errormsg, default);
        }


        // Method to execute a non-query asynchronously with parameters
        public static async Task<int> OldExecNonQueryAsync(string strSQL, List<Microsoft.Data.SqlClient.SqlParameter> parameters, string sref = "")
        {
            // Check if the SQL query is empty or null
            if (string.IsNullOrEmpty(strSQL))
            {
                // Log the error and return 0 if the SQL query is empty
                sys.writelog("sql_Emty_slow", "SQL query is empty" + Environment.StackTrace, true);
                return 0;
            }

            // Record the start time to measure execution duration
            DateTime start = DateTime.Now;
            int ret = 0;

            // Establish a new SQL connection using the connection string from ConHelper
            using (var sqlCn = new Microsoft.Data.SqlClient.SqlConnection(ConHelper.ConnectionConfiguration()))
            {
                try
                {
                    // Open the SQL connection asynchronously
                    await sqlCn.OpenAsync();

                    // Create a new SQL command with the provided SQL query and connection
                    using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(strSQL, sqlCn))
                    {
                        // Add parameters to the SQL command if any are provided
                        cmd.Parameters.AddRange(parameters.ToArray());
                        // Set command timeout to 1200 seconds (20 minutes)
                        cmd.CommandTimeout = 1200;

                        // Execute the non-query asynchronously and retrieve the number of rows affected
                        ret = await cmd.ExecuteNonQueryAsync();
                    }

                    // Close the SQL connection
                    sqlCn.Close();
                }
                catch (Exception ex)
                {
                    // Log the error with details of the SQL query and exception
                    sys.writelog("error_PCPMS_executenonquery", strSQL + "\r\n" + sys.ReadException(ex) + " / " + ex.StackTrace);

                    // Rethrow the exception to propagate it for further handling
                    throw;
                }
                finally
                {
                    // Ensure the SQL connection is closed if it's still open
                    if (sqlCn.State == System.Data.ConnectionState.Open)
                    {
                        sqlCn.Close();
                    }
                }
            }

            // Log slow queries if execution time exceeds 10 seconds
            if ((DateTime.Now - start).TotalSeconds > 10)
            {
                Console.WriteLine($"{(DateTime.Now - start).TotalSeconds} secs - {sref} - {strSQL}");
            }

            // Return the number of rows affected by the non-query execution
            return ret;
        }


        // Method to execute a non-query asynchronously with parameters and return status and error message
        public static async Task<(string err, int retcnt)> ExecNonQueryAsync(string strSQL, List<Microsoft.Data.SqlClient.SqlParameter> _params, string sref = "")
        {
            // Check if the SQL query is empty
            if (string.IsNullOrEmpty(strSQL))
            {
                // Log the error if the SQL query is empty
                sys.writelog("error_PCPMS_executenonquery", "sql is empty /" + Environment.StackTrace);
                return ("sql is empty", 0);
            }

            // Record the start time to measure execution duration
            DateTime start = DateTime.Now;
            int ret = 0;

            using (var sqlCn = new Microsoft.Data.SqlClient.SqlConnection(ConHelper.ConnectionConfiguration()))
            {
                try
                {
                    // Open the SQL connection
                    sqlCn.Open();

                    // Create a new SQL command with the provided SQL query and connection
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(strSQL, sqlCn);

                    // Add parameters to the SQL command
                    foreach (var p in _params)
                    {
                        cmd.Parameters.Add(p);
                    }

                    // Set command timeout to 1200 seconds (20 minutes)
                    cmd.CommandTimeout = 1200;

                    // Execute the non-query asynchronously and retrieve the number of rows affected
                    ret = await cmd.ExecuteNonQueryAsync();

                    // Close the SQL connection
                    sqlCn.Close();
                }
                catch (Exception ex)
                {
                    // Log the error with details of the SQL query and exception
                    sys.writelog("error_PCPMS_executenonquery", strSQL + "\r\n" + sys.ReadException(ex) + " / " + ex.StackTrace);

                    try
                    {
                        // Try to close the SQL connection in case of an exception
                        sqlCn.Close();
                    }
                    catch
                    {
                        // Handle any errors that might occur while closing the connection
                    }

                    // Return an error message with the exception details
                    return ("Error: " + ex.Message, 0);
                }

                // Log if no rows were affected by the non-query execution
                if (ret == 0)
                {
                    sys.writelog("executenonquery_PCPMS_zero", sref + " - " + strSQL + "\r\n" + Environment.StackTrace);
                }
            }

            // Log slow queries if execution time exceeds 10 seconds
            if ((DateTime.Now - start).TotalSeconds > 10)
            {
                sys.writelog("sqlREcom_slow", (DateTime.Now - start).TotalSeconds.ToString() + "secs " + sref + " - " + strSQL + "\r\n" + Environment.StackTrace, true);
            }

            // Return an empty error message and the number of rows affected
            return ("", ret);
        }


        // Executes a scalar SQL query asynchronously and returns the result as an integer
        public static async Task<int> ExecuteScalarAsync(string query, List<Microsoft.Data.SqlClient.SqlParameter> parameters = null, string sref = "")
        {
            // Check if the SQL query is empty or null
            if (string.IsNullOrEmpty(query))
            {
                // Log the error if the SQL query is empty and return 0
                sys.writelog("error_PCPMS_executescalar", "SQL query is empty /" + Environment.StackTrace);
                return 0;
            }

            // Record the start time to measure execution duration
            DateTime start = DateTime.Now;
            int result = 0;

            // Establish a new SQL connection using the connection string from ConHelper
            using (var sqlCn = new Microsoft.Data.SqlClient.SqlConnection(ConHelper.ConnectionConfiguration()))
            {
                try
                {
                    // Open the SQL connection asynchronously
                    await sqlCn.OpenAsync();

                    // Create a new SQL command with the provided SQL query and connection
                    using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(query, sqlCn))
                    {
                        // Add parameters to the SQL command if provided
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }
                        }

                        // Execute the scalar query asynchronously and retrieve the result
                        object scalarResult = await cmd.ExecuteScalarAsync();
                        result = Convert.ToInt32(scalarResult); // Convert the scalar result to an integer
                    }
                }
                catch (Exception ex)
                {
                    // Log the error with details of the SQL query and exception
                    sys.writelog("error_PCPMS_executescalar", $"{query}\r\n{sys.ReadException(ex)} / {ex.StackTrace}");

                    // Attempt to close the SQL connection if an exception occurs
                    try
                    {
                        sqlCn.Close();
                    }
                    catch
                    {
                        // Handle any exceptions that occur during closing if needed
                    }

                    // Re-throw the exception to propagate it up the call stack
                    throw;
                }
            }

            // Log slow queries if execution time exceeds 10 seconds
            if ((DateTime.Now - start).TotalSeconds > 10)
            {
                sys.writelog("sql_PCPMS_slow", $"{(DateTime.Now - start).TotalSeconds} secs {sref} - {query}\r\n{Environment.StackTrace}", true);
            }

            // Return the scalar result as an integer
            return result;
        }



        // Method to perform bulk insert asynchronously into a specified table
        public static async Task<bool> BulkInsert(string tableName, System.Data.DataTable dt)
        {
            var isError = false; // Flag to track if an error occurred
            var connectionString = ConHelper.ConnectionConfiguration(); // Get the connection string from helper

            try
            {
                using (var sqlCn = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    await sqlCn.OpenAsync(); // Open the SQL connection asynchronously

                    // Begin a SQL transaction
                    using (var transaction = sqlCn.BeginTransaction())
                    {
                        try
                        {
                            // Initialize SqlBulkCopy within the transaction
                            using (var sqlbc = new Microsoft.Data.SqlClient.SqlBulkCopy(sqlCn, SqlBulkCopyOptions.Default, transaction))
                            {
                                sqlbc.DestinationTableName = tableName; // Set the destination table
                                sqlbc.BulkCopyTimeout = 72000; // Set timeout for bulk copy operation
                                sqlbc.BatchSize = 20000; // Set the batch size for bulk copy
                                await sqlbc.WriteToServerAsync(dt); // Write data to server asynchronously
                            }

                            // Commit the transaction if everything is successful
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Log error if bulk insert operation fails
                            sys.writelog("error_PCPMS_bulkinsert", tableName + "\r\n" + sys.ReadException(ex) + " / " + ex.StackTrace);
                            isError = true; // Set error flag

                            try
                            {
                                // Attempt to rollback the transaction if an error occurs
                                transaction.Rollback();
                            }
                            catch (Exception exRollback)
                            {
                                // Log rollback error if rollback fails
                                sys.writelog("error_PCPMS_bulkinsert_rollback", tableName + "\r\n" + sys.ReadException(exRollback) + " / " + exRollback.StackTrace);
                            }
                        }
                    }

                    // Close the SQL connection after transaction completion
                    sqlCn.Close();
                }
            }
            catch (Exception ex)
            {
                // Log error if there's an exception at the outer try-catch level (connection open/close etc.)
                sys.writelog("error_PCPMS_bulkinsert", tableName + "\r\n" + sys.ReadException(ex) + " / " + ex.StackTrace);
                isError = true; // Set error flag
            }

            return isError; // Return whether an error occurred during the bulk insert operation
        }



    }
}
