using System.Data.SQLite;

namespace ProductManagements
{
    internal class ProductRepository
    {
        private readonly Database _database;
        private readonly Logger _logger = Logger.Instance; // ADD THIS LINE

        public ProductRepository()
        {
            _database = new Database();
            _logger.LogEvent("REPOSITORY_INIT", "ProductRepository initialized"); // ADD THIS LINE
        }

        // Create a new product
        public void AddProduct(Product product)
        {
            try // ADD TRY-CATCH
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO Products (Name, StockQuantity, Description, Price) VALUES (@Name, @StockQuantity, @Description, @Price)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@Price", product.Price);

                        int rowsAffected = command.ExecuteNonQuery(); // CHANGE THIS LINE

                        // ADD LOGGING AFTER SUCCESSFUL INSERT
                        if (rowsAffected > 0)
                        {
                            _logger.LogProductAdded($"{product.Name} (Stock: {product.StockQuantity}, Price: {product.Price:C})");
                        }
                        else
                        {
                            _logger.LogError($"Failed to add product: {product.Name} - No rows affected");
                        }
                    }
                }
            }
            catch (Exception ex) // ADD EXCEPTION HANDLING
            {
                _logger.LogError($"Error adding product '{product?.Name}': {ex.Message}");
                throw; // Re-throw so UI still handles the error
            }
        }

        // Read all products 
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            try // ADD TRY-CATCH
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Products";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"])
                            });
                        }
                    }
                }

                _logger.LogEvent("PRODUCTS_RETRIEVED", $"Retrieved {products.Count} products from database"); // ADD THIS LINE
                return products;
            }
            catch (Exception ex) // ADD EXCEPTION HANDLING
            {
                _logger.LogError($"Error retrieving products: {ex.Message}");
                throw;
            }
        }

        // Update product
        public void UpdateProduct(Product product)
        {
            try // ADD TRY-CATCH
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE Products SET Name = @Name, StockQuantity = @StockQuantity, Description = @Description, Price = @Price WHERE Id = @Id";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", product.Id);
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@Price", product.Price);

                        int rowsAffected = command.ExecuteNonQuery(); // CHANGE THIS LINE

                        // ADD LOGGING AFTER SUCCESSFUL UPDATE
                        if (rowsAffected > 0)
                        {
                            _logger.LogProductUpdated($"{product.Name} (ID: {product.Id}, Stock: {product.StockQuantity}, Price: {product.Price:C})");
                        }
                        else
                        {
                            _logger.LogError($"Failed to update product: {product.Name} - No rows affected (ID: {product.Id})");
                        }
                    }
                }
            }
            catch (Exception ex) // ADD EXCEPTION HANDLING
            {
                _logger.LogError($"Error updating product '{product?.Name}' (ID: {product?.Id}): {ex.Message}");
                throw;
            }
        }

        // Delete a product
        public void DeleteProduct(int productId)
        {
            try // ADD TRY-CATCH
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM Products WHERE Id = @Id";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", productId);
                        int rowsAffected = command.ExecuteNonQuery(); // CHANGE THIS LINE

                        // ADD LOGGING AFTER SUCCESSFUL DELETE
                        if (rowsAffected > 0)
                        {
                            _logger.LogProductDeleted($"Product ID: {productId}");
                        }
                        else
                        {
                            _logger.LogError($"Failed to delete product with ID: {productId} - No rows affected");
                        }
                    }
                }
            }
            catch (Exception ex) // ADD EXCEPTION HANDLING
            {
                _logger.LogError($"Error deleting product with ID {productId}: {ex.Message}");
                throw;
            }
        }
    }
}