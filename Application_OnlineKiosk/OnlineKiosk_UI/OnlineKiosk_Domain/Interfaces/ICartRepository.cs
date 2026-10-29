using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineKiosk_Domain.Data;

namespace OnlineKiosk_Domain.Interfaces
{
    public interface ICartRepository
    {
        public int CompleteOrder(Cart cart);
        public ProductVariant? GetProductVariantById(int id);
        public List<Product> GetProductsInCart(List<CartItem> cartItems);
    }
}
