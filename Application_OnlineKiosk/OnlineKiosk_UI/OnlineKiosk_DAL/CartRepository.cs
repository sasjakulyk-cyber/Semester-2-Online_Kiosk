using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int StoreCart(Cart cart)
        {
            int cartId = 0;
            using var connection = new SqlConnection(_connectionString);

            // Implementation for storing cart
            try
            {
                connection.Open();
                connection.BeginTransaction();

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
                    itemCommand.Parameters.AddWithValue("@ProductVariantId", item.Item.ProductVariantId);
                    itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                    itemCommand.ExecuteNonQuery();
                }
                connection.Commit();
            }
            catch (SqlException ex)
            {
                // ToDo: Implement proper error handling and logging
                _logger.LogError(ex, ex.Message);
                connection.Rollback();
                throw ex;
            }

            return cartId;
        }
    }
}
