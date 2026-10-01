using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Logic
{
    public interface IProductService
    {
        Task<List<Category>> GetCategories();
        Task<List<Product>> GetProductsByCategory(int categoryId);
        Task<Product?> GetProductDetailsByProductId(int productId);
    }
}
