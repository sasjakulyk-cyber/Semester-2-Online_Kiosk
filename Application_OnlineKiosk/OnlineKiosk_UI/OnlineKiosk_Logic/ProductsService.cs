using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Logic
{
    public class ProductsService : IProductService
    {
        private readonly IProductRepository _ProductRepository;

        public ProductsService(IProductRepository productRepository)
        {
            _ProductRepository = productRepository;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _ProductRepository.GetCategories();
        }

        public async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
            return await _ProductRepository.GetProductsByCategory(categoryId);
        }

        public async Task<Product?> GetProductDetailsByProductId(int productId)
        {
            return await _ProductRepository.GetProductDetailsByProductId(productId);
        }
    }
}
