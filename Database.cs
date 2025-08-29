using System.Data.SQLite;

namespace ProductManagements
{
    internal class Database
    {
        private readonly string ConnectionString;
        private readonly Logger _logger = Logger.Instance;

        public Database()
        {
            // Get the directory where the exe is located
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            string dbPath = Path.Combine(exeDirectory, "products.db");
            bool isNewDatabase = false;

            // Create the database file if it doesn't exist
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                isNewDatabase = true;
                _logger.LogEvent("DATABASE_FILE_CREATION", $"Database file created at: {dbPath}");
            }

            ConnectionString = $"Data Source={dbPath};Version=3;";

            // Always ensure tables exist
            CreateTables();

            // Handle migrations
            HandleMigrations();

            // Log database initialization
            if (isNewDatabase)
            {
                _logger.LogDatabaseCreation();
            }
            else
            {
                _logger.LogEvent("DATABASE_CONNECTION", $"Connected to existing database at: {dbPath}");
            }
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        private void CreateTables()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Create Users table with additional columns
                    string createUsersTable = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL UNIQUE,
                            Password TEXT NOT NULL,
                            Telephone TEXT,
                            Address TEXT
                        )";
                    using (var command = new SQLiteCommand(createUsersTable, connection))
                    {
                        command.ExecuteNonQuery();
                        _logger.LogEvent("TABLE_CREATION", "Users table created/verified");
                    }

                    // Insert test data if table is empty
                    string checkDataQuery = "SELECT COUNT(*) FROM Users";
                    using (var command = new SQLiteCommand(checkDataQuery, connection))
                    {
                        long count = (long)command.ExecuteScalar();
                        if (count == 0)
                        {
                            // Insert sample test data
                            string insertTestData = @"
                                INSERT INTO Users (Username, Password, Telephone, Address) VALUES
                                ('testuser', 'password123', '123456890', 'Test Address'),
                                ('nadia', 'nadia123', '25365441', 'address1'),
                                ('taleloueslati', 'talel123', '55695869', 'mourouj')";
                            using (var insertCommand = new SQLiteCommand(insertTestData, connection))
                            {
                                insertCommand.ExecuteNonQuery();
                                _logger.LogEvent("TEST_DATA_INSERTION", "Sample user data inserted into Users table");
                            }
                        }
                    }

                    // Create Products table (if needed)
                    string createProductsTable = @"
                        CREATE TABLE IF NOT EXISTS Products (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            StockQuantity INTEGER DEFAULT 0,
                            Description TEXT,     
                            Price REAL NOT NULL    
                        )";
                    using (var command = new SQLiteCommand(createProductsTable, connection))
                    {
                        command.ExecuteNonQuery();
                        _logger.LogEvent("TABLE_CREATION", "Products table created/verified");
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during database table creation: {ex.Message}");
                    throw;
                }
            }
        }

        private void HandleMigrations()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Check if StockQuantity column exists in Products table
                    if (!ColumnExists(connection, "Products", "StockQuantity"))
                    {
                        _logger.LogEvent("SCHEMA_MIGRATION", "Adding missing StockQuantity column to Products table");
                        // Add the StockQuantity column
                        string addColumnQuery = "ALTER TABLE Products ADD COLUMN StockQuantity INTEGER DEFAULT 0";
                        using (var command = new SQLiteCommand(addColumnQuery, connection))
                        {
                            command.ExecuteNonQuery();
                            _logger.LogEvent("SCHEMA_MIGRATION", "Successfully added StockQuantity column");
                        }
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during schema migration: {ex.Message}");
                    throw;
                }
            }
        }

        private bool ColumnExists(SQLiteConnection connection, string tableName, string columnName)
        {
            string query = $"PRAGMA table_info({tableName})";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["name"].ToString().Equals(columnName, System.StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}