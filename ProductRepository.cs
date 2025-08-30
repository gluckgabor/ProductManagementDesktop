using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace ProductManagements
{
    public class ProductRepository
    {
        private readonly string _connectionString;
        private readonly string _dbPath;

        public ProductRepository()
        {
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    // Check if Products table exists and get its schema
                    string checkTableSql = "SELECT sql FROM sqlite_master WHERE type='table' AND name='Products'";
                    using (var checkCommand = new SQLiteCommand(checkTableSql, connection))
                    {
                        var tableSchema = checkCommand.ExecuteScalar()?.ToString();

                        if (tableSchema != null && !tableSchema.Contains("CreatedDate"))
                        {
                            // Table exists but doesn't have CreatedDate - add it
                            string alterSql = "ALTER TABLE Products ADD COLUMN CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP";
                            using (var alterCommand = new SQLiteCommand(alterSql, connection))
                            {
                                alterCommand.ExecuteNonQuery();
                            }
                        }
                        else if (tableSchema == null)
                        {
                            // Table doesn't exist - create it
                            string createTableSql = @"
                                CREATE TABLE Products (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT NOT NULL,
                                    Description TEXT,
                                    StockQuantity INTEGER NOT NULL DEFAULT 0,
                                    Price DECIMAL(10,2) NOT NULL,
                                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                                )";

                            using (var command = new SQLiteCommand(createTableSql, connection))
                            {
                                command.ExecuteNonQuery();
                            }

                            // Add sample data if table is empty
                            AddSampleData(connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Hiba az adatbázis inicializálásakor: {ex.Message}", ex);
            }
        }

        private void AddSampleData(SQLiteConnection connection)
        {
            string[] sampleProducts = {
                "INSERT INTO Products (Name, Description, StockQuantity, Price) VALUES ('Laptop', 'Gaming laptop', 5, 299999.00)",
                "INSERT INTO Products (Name, Description, StockQuantity, Price) VALUES ('Egér', 'Vezeték nélküli egér', 20, 7999.99)",
                "INSERT INTO Products (Name, Description, StockQuantity, Price) VALUES ('Billentyűzet', 'Mechanikus billentyűzet', 15, 25000.50)"
            };

            foreach (string sql in sampleProducts)
            {
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT Id, Name, Description, StockQuantity, Price, CreatedDate FROM Products ORDER BY Id";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString(),
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedDate"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Hiba a termékek lekérdezésekor: {ex.Message}", ex);
            }

            return products;
        }

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("A terméknév nem lehet üres.", nameof(product.Name));

            try
            {
                // BUG #3: Missing duplicate name check!
                // This should be uncommented for the fix:
                /*
                if (IsProductNameExists(product.Name))
                {
                    throw new InvalidOperationException($"A '{product.Name}' nevű termék már létezik az adatbázisban!");
                }
                */

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sql = @"INSERT INTO Products (Name, Description, StockQuantity, Price, CreatedDate) 
                                 VALUES (@name, @description, @stockQuantity, @price, @createdDate)";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@name", System.Data.DbType.String).Value = product.Name;
                        command.Parameters.Add("@description", System.Data.DbType.String).Value = product.Description ?? string.Empty;
                        command.Parameters.Add("@stockQuantity", System.Data.DbType.Int32).Value = product.StockQuantity;
                        command.Parameters.Add("@price", System.Data.DbType.Decimal).Value = product.Price;
                        command.Parameters.Add("@createdDate", System.Data.DbType.DateTime).Value = DateTime.Now;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new InvalidOperationException($"Adatbázis hiba a termék hozzáadásakor: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Hiba a termék hozzáadásakor: {ex.Message}", ex);
            }
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("A terméknév nem lehet üres.", nameof(product.Name));

            try
            {
                // BUG #3: Missing duplicate name check for updates too!
                /*
                if (IsProductNameExistsForUpdate(product.Name, product.Id))
                {
                    throw new InvalidOperationException($"A '{product.Name}' nevű termék már létezik az adatbázisban!");
                }
                */

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sql = @"UPDATE Products 
                                 SET Name = @name, Description = @description, 
                                     StockQuantity = @stockQuantity, Price = @price
                                 WHERE Id = @id";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@id", System.Data.DbType.Int32).Value = product.Id;
                        command.Parameters.Add("@name", System.Data.DbType.String).Value = product.Name;
                        command.Parameters.Add("@description", System.Data.DbType.String).Value = product.Description ?? string.Empty;
                        command.Parameters.Add("@stockQuantity", System.Data.DbType.Int32).Value = product.StockQuantity;
                        command.Parameters.Add("@price", System.Data.DbType.Decimal).Value = product.Price;

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException($"A termék (ID: {product.Id}) nem található.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new InvalidOperationException($"Adatbázis hiba a termék frissítésekor: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Hiba a termék frissítésekor: {ex.Message}", ex);
            }
        }

        public void DeleteProduct(int productId)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM Products WHERE Id = @id";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@id", System.Data.DbType.Int32).Value = productId;

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException($"A termék (ID: {productId}) nem található.");
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new InvalidOperationException($"Adatbázis hiba a termék törlésekor: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Hiba a termék törlésekor: {ex.Message}", ex);
            }
        }

        public Product GetProductById(int productId)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT Id, Name, Description, StockQuantity, Price, CreatedDate FROM Products WHERE Id = @id";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@id", System.Data.DbType.Int32).Value = productId;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Product
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString(),
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedDate"])
                                };
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Hiba a termék lekérdezésekor: {ex.Message}", ex);
            }
        }

        // These methods are commented out to create BUG #3
        // Uncomment for the fix version:
        /*
        private bool IsProductNameExists(string productName)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM Products WHERE LOWER(TRIM(Name)) = LOWER(TRIM(@name))";
                    
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@name", System.Data.DbType.String).Value = productName;
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Hiba a terméknév ellenőrzésekor: {ex.Message}", ex);
            }
        }

        private bool IsProductNameExistsForUpdate(string productName, int excludeProductId)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM Products WHERE LOWER(TRIM(Name)) = LOWER(TRIM(@name)) AND Id != @id";
                    
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.Add("@name", System.Data.DbType.String).Value = productName;
                        command.Parameters.Add("@id", System.Data.DbType.Int32).Value = excludeProductId;
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Hiba a terméknév ellenőrzésekor: {ex.Message}", ex);
            }
        }
        */
    }
}