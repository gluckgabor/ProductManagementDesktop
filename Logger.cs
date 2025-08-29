namespace ProductManagements
{
    public class Logger
    {
        private static readonly object _lockObject = new object();
        private static Logger _instance;

        // Singleton pattern to ensure one logger instance
        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                            _instance = new Logger();
                    }
                }
                return _instance;
            }
        }

        private Logger() { }

        /// <summary>
        /// Logs an event to the daily log file
        /// </summary>
        /// <param name="eventType">Type of event (e.g., "REGISTRATION", "ADD_PRODUCT", etc.)</param>
        /// <param name="message">Detailed message about the event</param>
        /// <param name="username">Username associated with the event (optional)</param>
        public void LogEvent(string eventType, string message, string username = null)
        {
            try
            {
                string logFileName = GetLogFileName();
                string logEntry = CreateLogEntry(eventType, message, username);

                lock (_lockObject)
                {
                    // Append to existing file or create new one
                    File.AppendAllText(logFileName, logEntry + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // If logging fails, we can't log the error, so write to console as fallback
                Console.WriteLine($"Logging error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the log file name for today
        /// </summary>
        /// <returns>Log file name in format log_YYYY-MM-DD.txt</returns>
        private string GetLogFileName()
        {
            string dateString = DateTime.Now.ToString("yyyy-MM-dd");
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            return Path.Combine(exeDirectory, $"log_{dateString}.txt");
        }

        /// <summary>
        /// Creates a formatted log entry
        /// </summary>
        /// <param name="eventType">Type of event</param>
        /// <param name="message">Event message</param>
        /// <param name="username">Username (optional)</param>
        /// <returns>Formatted log entry</returns>
        private string CreateLogEntry(string eventType, string message, string username)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userInfo = string.IsNullOrEmpty(username) ? "" : $" | User: {username}";

            return $"[{timestamp}] [{eventType}]{userInfo} - {message}";
        }

        /// <summary>
        /// Log database creation event
        /// </summary>
        public void LogDatabaseCreation()
        {
            LogEvent("DATABASE_CREATION", "Database and tables created successfully");
        }

        /// <summary>
        /// Log user registration event
        /// </summary>
        /// <param name="username">Username that was registered</param>
        public void LogUserRegistration(string username)
        {
            LogEvent("REGISTRATION", $"New user registered: {username}", username);
        }

        /// <summary>
        /// Log user login event
        /// </summary>
        /// <param name="username">Username that logged in</param>
        public void LogUserLogin(string username)
        {
            LogEvent("LOGIN", $"User logged in successfully", username);
        }

        /// <summary>
        /// Log failed login attempt
        /// </summary>
        /// <param name="username">Username that attempted to log in</param>
        public void LogFailedLogin(string username)
        {
            LogEvent("LOGIN_FAILED", $"Failed login attempt for username: {username}", username);
        }

        /// <summary>
        /// Log product addition event
        /// </summary>
        /// <param name="productName">Name of the product added</param>
        /// <param name="username">User who added the product</param>
        public void LogProductAdded(string productName, string username = null)
        {
            LogEvent("ADD_PRODUCT", $"Product added: {productName}", username);
        }

        /// <summary>
        /// Log product update event
        /// </summary>
        /// <param name="productName">Name of the product updated</param>
        /// <param name="username">User who updated the product</param>
        public void LogProductUpdated(string productName, string username = null)
        {
            LogEvent("UPDATE_PRODUCT", $"Product updated: {productName}", username);
        }

        /// <summary>
        /// Log product deletion event
        /// </summary>
        /// <param name="productName">Name of the product deleted</param>
        /// <param name="username">User who deleted the product</param>
        public void LogProductDeleted(string productName, string username = null)
        {
            LogEvent("DELETE_PRODUCT", $"Product deleted: {productName}", username);
        }

        /// <summary>
        /// Log application startup
        /// </summary>
        public void LogApplicationStart()
        {
            LogEvent("APPLICATION_START", "ProductManagements application started");
        }

        /// <summary>
        /// Log application shutdown
        /// </summary>
        public void LogApplicationShutdown()
        {
            LogEvent("APPLICATION_SHUTDOWN", "ProductManagements application closed");
        }

        /// <summary>
        /// Log general errors
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="username">User associated with the error (optional)</param>
        public void LogError(string errorMessage, string username = null)
        {
            LogEvent("ERROR", errorMessage, username);
        }
    }
}