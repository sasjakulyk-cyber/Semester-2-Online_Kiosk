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

        public List<Category> GetCategories()
        {
            try
            {
                return _ProductRepository.GetCategories();

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("An error occurred while retrieving categories.", ex);
            }
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _ProductRepository.GetProductsByCategory(categoryId);
        }

        public Product? GetProductDetailsByProductId(int productId)
        {
            return _ProductRepository.GetProductDetailsByProductId(productId);
        }
    }
}
