using Microsoft.Extensions.Configuration;

namespace bl
{
    public class ConHelper
    {
        // Method to retrieve connection settings for EComDB
        public static ConfigValue EComDB()
        {
            // Retrieve connection settings from configuration or elsewhere
            string _DBSERVER = GetString("dbserver");  // Get DB server name
            string _DBNAME = GetString("dbname");      // Get DB name
            string _DBUSER = GetString("dbuser");      // Get DB username
            string _DBPASS = GetString("dbpass");      // Get DB password

            // Create an instance of ConfigValue and populate it
            ConfigValue connectionString = new ConfigValue
            {
                DBSERVER = _DBSERVER,
                DBNAME = _DBNAME,
                DBUSER = _DBUSER,
                DBPASS = _DBPASS
            };

            // Return the instance of ConfigValue containing DB connection info
            return connectionString;
        }

        // Method to retrieve port settings
        public static ConfigValue MyPort()
        {
            string _Port = GetString("port");  // Get port number

            // Create an instance of ConfigValue and populate the port
            ConfigValue Myport = new ConfigValue
            {
                Port = _Port,
            };

            // Return the instance of ConfigValue containing port info
            return Myport;
        }


        public static ConfigValue SavefileImg()
        {
            // Retrieve a configuration value for the key "picture".
            string _SaveIMG = GetString("picture");

            // Create a new instance of ConfigValue and assign the retrieved value to its SaveImg property.
            ConfigValue SaveIMG = new ConfigValue
            {
                SaveImg = _SaveIMG,
            };

            // Return the configured ConfigValue object.
            return SaveIMG;
        }

        // Method to construct and return the connection string
        public static string ConnectionConfiguration()
        {
            // Get the database connection configuration
            ConfigValue config = EComDB();

            // Construct and return the connection string using retrieved configuration
            string connectionString = $@"
                Server={config.DBSERVER};
                Database={config.DBNAME};
                User={config.DBUSER};
                Password={config.DBPASS};
                TrustServerCertificate=True;";


            return connectionString;
        }

        // Method to retrieve string value from configuration using a key
        public static string GetString(string key)
        {
            // Build configuration from appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            // Retrieve the value for the provided key from appsettings.json
            string value = configuration[key];

            // If the value is null or empty, try the fallback file
            if (string.IsNullOrEmpty(value))
            {
                // Define fallback file path in the documents folder
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fallbackFilePath = Path.Combine(documentsPath, "PCPMS.json");

                if (File.Exists(fallbackFilePath))
                {
                    // Build configuration from PCPMS.json in the documents folder
                    var fallbackBuilder = new ConfigurationBuilder()
                        .SetBasePath(documentsPath)
                        .AddJsonFile("PCPMS.json", optional: true, reloadOnChange: true);

                    IConfiguration fallbackConfiguration = fallbackBuilder.Build();

                    // Retrieve the value from fallback configuration
                    value = fallbackConfiguration[key];
                }
            }

            return value;
        }

        // Class to hold configuration values
        public class ConfigValue
        {
            public string DBSERVER { get; set; }  // Property for database server name
            public string DBNAME { get; set; }    // Property for database name
            public string DBUSER { get; set; }    // Property for database username
            public string DBPASS { get; set; }    // Property for database password
            public string Port { get; set; }      // Property for port number
            public string SaveImg { get; set; }      // Property for port number
        }
    }
}
