using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Logic
{
    public interface IProductService
    {
        List<Category> GetCategories();
        List<Product> GetProductsByCategory(int categoryId);
        Product? GetProductDetailsByProductId(int productId);
    }
}
