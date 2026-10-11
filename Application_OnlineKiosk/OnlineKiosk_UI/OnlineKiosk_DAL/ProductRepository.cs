using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineKiosk_Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace OnlineKiosk_DAL
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration, ILogger<ProductRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing in configuration.");
            _logger = logger;
        }

        // Implement methods to interact with the database, e.g., GetProducts, AddProduct, etc.
        public List<Category> GetCategories()
        {
            var categories = new List<Category>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var query = "SELECT * FROM [dbo].[Category]";
                using var command = new SqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category
                    (
                        reader.GetInt32("Id"),
                        reader.GetString("categoryName"),
                        reader.IsDBNull("imageURL") ? string.Empty : reader.GetString("imageURL")
                    ));
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            return categories;
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            var products = new List<Product>();
            try 
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                // Select one variant per product — the one with the lowest price. Use CROSS APPLY to pick TOP 1
                // This produces deterministic results on SQL Server without requiring every selected column to be grouped.
                var query = "SELECT p.Id, p.title as Name, p.Description, p.ImageURL, " +
                            "pv.Id as VariantId, pv.variantDescription as VariantDescription, pv.Price as Price " +
                            "FROM [dbo].[Product] as p " +
                            "CROSS APPLY (" +
                            "   SELECT TOP 1 * FROM [dbo].[ProductVariant] pv2 " +
                            "   WHERE pv2.ProductId = p.Id ORDER BY pv2.Price ASC, pv2.Id ASC" +
                            ") pv " +
                            "WHERE p.CategoryId = @CategoryId";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product
                    (
                        reader.GetInt32("Id"),
                        reader.GetString("Name"),
                        reader.GetString("Description"),
                        reader.IsDBNull("ImageURL") ? string.Empty : reader.GetString("ImageURL")
                    );
                    product.AddVariant(
                        reader.GetInt32("VariantId"),
                        reader.GetString("VariantDescription"),
                        reader.GetDecimal("Price")
                    );
                    products.Add(product);
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            return products;
        }

        public Product? GetProductDetailsByProductId(int productId)
        {
            Product? product = null;
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var query = "SELECT p.Id, p.title as Name, p.Description, p.ImageURL, " +
                            "       pv.Id as VariantId, pv.variantDescription as VariantDescription, pv.Price as Price " +
                            "FROM [dbo].[Product] as p " +
                            "INNER JOIN [dbo].[ProductVariant] as pv ON p.Id = pv.ProductId " +
                            "WHERE p.Id = @ProductId";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (product == null)
                    {
                        product = new Product
                        (
                            reader.GetInt32("Id"),
                            reader.GetString("Name"),
                            reader.GetString("Description"),
                            reader.IsDBNull("ImageURL") ? string.Empty : reader.GetString("ImageURL")
                        );
                        _logger.LogWarning("Add new product {}", reader.GetInt32("Id"));
                    }
                    product.AddVariant(
                        reader.GetInt32("VariantId"),
                        reader.GetString("VariantDescription"),
                        reader.GetDecimal("Price")
                    );
                    _logger.LogWarning("Read variant {}", reader.GetInt32("VariantId"));
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            //product.AddAllergens(GetProductAllergensByProductId(productId))
            return product;
        }
    }
}
