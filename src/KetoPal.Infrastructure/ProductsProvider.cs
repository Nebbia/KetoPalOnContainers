using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using KetoPal.Core;
using KetoPal.Core.Models;

namespace KetoPal.Infrastructure
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly string _connectionString;

        public ProductsProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Product>> GetFoodProductsByCarbs()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // oh yea boil the ocean
                var products = await connection.QueryAsync<Product>("usp_Get_FoodProductsByCarbs", commandType: CommandType.StoredProcedure);

                return products.ToList();
            }
        }

        public async Task<List<Product>> GetFoodProductsByCarbsForUser(User user)
        {
            var products = await GetFoodProductsByCarbs();

            List<Product> productsThatCanBeConsumed = products.Where(product => user.CanConsume(product)).ToList();

            return productsThatCanBeConsumed;
        }
    }
}
