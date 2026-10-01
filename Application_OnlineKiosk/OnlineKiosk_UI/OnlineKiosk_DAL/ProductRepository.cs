using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using OnlineKiosk_Logic;

namespace OnlineKiosk_DAL
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentNullException(nameof(configuration), "Connection string is missing in configuration.");
            }

        }

        // Implement methods to interact with the database, e.g., GetProducts, AddProduct, etc.
        public async Task<List<Category>> GetCategories()
        {
            var categories = new List<Category>();


            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM [dbo].[Category]";
            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                (
                    reader.GetInt32("Id"),
                    reader.GetString("categoryName"),
                    reader.GetString("imageURL")
                ));
            }
            return categories;
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            var products = new List<Product>();
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
                    reader.GetString("ImageURL")
                );
                product.AddVariant(
                    reader.GetInt32("VariantId"),
                    reader.GetString("VariantDescription"),
                    reader.GetDouble("Price")
                );
                products.Add(product);
            }
            return products;
        }

        public Product? GetProductDetailsByProductId(int productId)
        {
            Product? product = null;
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = "SELECT p.Id, p.title as Name, p.Description, p.ImageURL, " +
                        "       pv.Id as VariantId, pv.variantDescription as VariantDescription, pv.Price as Price " +
                        "FROM [dbo].[Product] as p " +
                        "JOIN [dbo].[ProductVariant] as pv ON p.Id = pv.ProductId " +
                        "WHERE p.Id = @ProductId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (product == null)
                {
                    product = new Product
                    (
                        reader.GetInt32("Id"),
                        reader.GetString("Name"),
                        reader.GetString("Description"),
                        reader.GetString("ImageURL")
                    );
                }
                product.AddVariant(
                    reader.GetInt32("VariantId"),
                    reader.GetString("VariantDescription"),
                    reader.GetDouble("Price")
                );
            }
            return product;
        }
    }
}
