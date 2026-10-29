using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineKiosk_Domain.Data;
using OnlineKiosk_Domain.Interfaces;
using System.Data;

namespace OnlineKiosk_DAL
{
    public class CartRepository : ICartRepository
    {
        private readonly ILogger<CartRepository> _logger;
        private readonly string _connectionString;

        public CartRepository(IConfiguration configuration, ILogger<CartRepository> logger)
        {
            // ToDo: change configuration as input in the constructor to get connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing in configuration.");
            _logger = logger;
        }

        public int CompleteOrder(Cart cart)
        {
            int cartId = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    // Implementation for storing cart
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw new Exception("An error occurred while connecting to the database.", ex);
                }

                var transaction = connection.BeginTransaction();

                // ToDo: Extend Cart in DB with subtotal and Total Price for integrity verification and performance improvement.
                // Prevent cases where the price of a product is changed after the cart is created and before the order is placed.
                // This will ensure that the price of the product at the time of adding to the cart is preserved.
                // ToDo: Extend CartItem in DB with price for integrity verification and performance improvement.
                try
                {
                    // Insert cart into the database
                    var insertCartQuery = "INSERT INTO [dbo].[Cart] (c  reated_at) VALUES (CURRENT_TIMESTAMP()); SELECT SCOPE_IDENTITY();";
                    using var command = new SqlCommand(insertCartQuery, connection);
                    cartId = Convert.ToInt32(command.ExecuteScalar());

                    var insertCartItemQuery = "INSERT INTO [dbo].[CartItem] (CartId, ProductVariantId, quantity) VALUES (@CartId, @ProductVariantId, @Quantity)";
                    using var itemCommand = new SqlCommand(insertCartItemQuery, connection);
                    // ToDo: Find batch insert syntax for inserting multiple rows in one query to improve performance
                    foreach (var item in cart.Items)
                    {
                        itemCommand.Parameters.Clear();
                        itemCommand.Parameters.AddWithValue("@CartId", cartId);
                        itemCommand.Parameters.AddWithValue("@ProductVariantId", item.VariantId);
                        itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                        itemCommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    // ToDo: Implement proper error handling and logging
                    _logger.LogError(ex, ex.Message);
                    transaction.Rollback();
                    throw new Exception("An error occurred while storing the cart.", ex);
                }
            }

            return cartId;
        }

        public ProductVariant? GetProductVariantById(int id)
        {
            ProductVariant? productVariant = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var query = "SELECT pv.* FROM [dbo].[ProductVariant] pv " +
                                "INNER JOIN [dbo].[Product] p ON pv.ProductId = p.Id " +
                            "WHERE pv.Id = @Id";
                    using var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        productVariant = new ProductVariant(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("variantDescription")),
                            reader.GetDecimal(reader.GetOrdinal("price")),
                            reader.GetInt32(reader.GetOrdinal("ProductId"))
                        );
                    }
                }
                catch (SqlException ex)
                {
                    // ToDo: Implement proper error handling and logging
                    _logger.LogError(ex, ex.Message);
                    throw new Exception("An error occurred while retrieving the product variant.", ex);
                }
            }
            return productVariant;
        }

        public List<Product> GetProductsInCart(List<CartItem> cartItems)
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var query = "SELECT p.Id, p.title as Name, p.Description, p.ImageURL, " +
                                "       pv.Id as VariantId, pv.variantDescription as VariantDescription, pv.Price as Price " +
                            "FROM [dbo].[Product] as p " +
                            "INNER JOIN [dbo].[ProductVariant] as pv ON p.Id = pv.ProductId " +
                            "WHERE pv.Id IN (" + string.Join(",", cartItems.Select(i => i.VariantId)) + ")"; //rework
                    using var command = new SqlCommand(query, connection);
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var product = new Product(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Description")),
                                reader.IsDBNull(reader.GetOrdinal("ImageURL")) ? string.Empty : reader.GetString(reader.GetOrdinal("ImageURL"))
                            );
                        product.AddVariant(
                            reader.GetInt32(reader.GetOrdinal("VariantId")),
                            reader.GetString(reader.GetOrdinal("VariantDescription")),
                            reader.GetDecimal(reader.GetOrdinal("Price"))
                        );

                        products.Add(product);
                    }
                }
                catch (SqlException ex)
                {
                    // ToDo: Implement proper error handling and logging
                    _logger.LogError(ex, ex.Message);
                    throw new Exception("An error occurred while retrieving the products in the cart.", ex);
                }
            }
            return products;
        }
    }
}