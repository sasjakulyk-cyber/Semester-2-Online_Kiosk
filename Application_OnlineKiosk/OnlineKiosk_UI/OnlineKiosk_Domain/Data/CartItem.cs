using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Domain.Data
{
    public class CartItem
    {
        public int CartItemId { get; private set; }
        public int VariantId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; } // Price of the product variant at the time it was added to the cart

        // ToDo: Make sure all cases of use of Price is not confused with varian.Price.
        //       And make sure the Price from Variant is always passed properly to CartItem

        public CartItem(int cartItemId, int variantId, int quantity, decimal price)
        {
            CartItemId = cartItemId;
            Quantity = quantity;
            VariantId = variantId;
            Price = price;
        }

        public void IncreaseQuantity(int amount = 1)
        {
            Quantity += amount;
        }

        public void DecreaseQuantity(int amount = 1)
        {
            Quantity -= amount;
        }

        public bool needToRemove()
        {
            return Quantity <= 0;
        }

        public decimal SubTotal()
        {
            return Price * Quantity;
        }
    }
}
