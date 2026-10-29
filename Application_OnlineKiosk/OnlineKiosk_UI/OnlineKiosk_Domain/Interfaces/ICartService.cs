using OnlineKiosk_Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Domain.Interfaces
{
    public interface ICartService
    {
        public void IncreaseQuantity(int variantId, int quantity);
        public bool DecreaseQuantity(int variantId, int quantity);
        public void RemoveItem(int variantId);
        public void SaveCart();
        public void CompleteOrder();
        public Product getProductByCartItem(CartItem cartItem);
        public Cart GetCart();
        public decimal GetSubtotal();
        public decimal GetTotal();
    }
}
