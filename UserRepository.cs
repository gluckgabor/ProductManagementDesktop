using System.Data.SQLite;

namespace ProductManagements
{
    internal class UserRepository
    {
        private readonly Database _database;
        private readonly Logger _logger = Logger.Instance;

        public UserRepository()
        {
            _database = new Database();
            _logger.LogEvent("REPOSITORY_INIT", "UserRepository initialized");
        }

        public bool ValidateUser(string username, string password)
        {
            try
            {
                _logger.LogEvent("LOGIN_ATTEMPT", $"Login attempt for username: {username}");

                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        var result = (long)command.ExecuteScalar();

                        bool isValid = result > 0;

                        if (isValid)
                        {
                            _logger.LogUserLogin(username);
                        }
                        else
                        {
                            _logger.LogFailedLogin(username);
                        }

                        return isValid;
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error validating user '{username}': {ex.Message}");
                throw; // Re-throw to let UI handle the error display
            }
        }
    }
}