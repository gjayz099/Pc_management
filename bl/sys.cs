namespace bl
{
    public class sys
    {
        public static LogPath logpath()
        {
            // Retrieve log path from configuration using helper method GetString from ConHelper class
            string _Logpath = bl.ConHelper.GetString("logPath");

            // Create an instance of LogPath and assign the retrieved log path
            LogPath Logpath = new LogPath
            {
                Logpath = _Logpath,
            };

            // Return the LogPath instance containing the log path
            return Logpath;
        }


    
        /// <summary>
        /// Writes a log entry to the log file.
        /// </summary>
        /// <param name="logCategory">Category of the log entry.</param>
        /// <param name="logMessage">Message to be logged.</param>
        /// <param name="append">True to append to the log file, false to overwrite.</param>
        public static void writelog(string logCategory, string logMessage, bool append)
        {
            // Retrieve log path from logpath() method
            LogPath logPathObj = logpath();
            string logDirectory = logPathObj.Logpath;

            try
            {
                // Construct log file name with current date
                string logFileName = $"log_{DateTime.Now.ToString("yyyyMMdd")}.txt";
                string logFilePath = System.IO.Path.Combine(logDirectory, logFileName);

                // Format log entry with timestamp
                string logEntry = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [{logCategory}] {logMessage}";

                // Write to log file
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(logFilePath, append))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                // Handle log write exception if necessary
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        /// <summary>
        /// Writes an error log entry to the error log file.
        /// </summary>
        /// <param name="logCategory">Category of the error log entry.</param>
        /// <param name="logMessage">Error message to be logged.</param>
        public static void writelog(string logCategory, string logMessage)
        {
            // Retrieve log path from logpath() method
            LogPath logPathObj = logpath();
            string logDirectory = logPathObj.Logpath;

            try
            {

                // Construct error log file name with current date
                string logFileName = $"log_error{DateTime.Now.ToString("yyyyMMdd")}.txt";
                string logFilePath = Path.Combine(logDirectory, logFileName);

                // Format error log entry with timestamp
                string logEntry = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [{logCategory}] {logMessage}";

                // Write to error log file
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                // Handle log write exception if necessary
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        public static void Acceslog(string logName, string logUser, string logAccess)
        {
            LogPath logPathObj = logpath();
            string logDirectory = logPathObj.Logpath;

            try
            {
                // Construct error log file name with current date
                string logFileName = $"log_{logName}{DateTime.Now.ToString("yyyyMMdd")}.txt";
                string logFilePath = Path.Combine(logDirectory, logFileName);

                // Format error log entry with timestamp
                string logEntry = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {logUser} {logAccess}";

                // Write to error log file
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                // Handle log write exception if necessary
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        /// <summary>
        /// Recursively reads the exception message including inner exceptions.
        /// </summary>
        /// <param name="ex">Exception object to read.</param>
        /// <returns>Exception message including all inner exceptions.</returns>
        public static string ReadException(Exception ex)
        {
            string message = ex.Message;

            // Recursively read inner exceptions if present
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                message += " / " + ex.Message;
            }

            return message;
        }

        // Class to hold log path configuration
        public class LogPath
        {
            public string Logpath { get; set; }  // Property for log file path
        }
    }
}
