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

        public CartItem(int cartItemId, int variantId, int quantity)
        {
            CartItemId = cartItemId;
            Quantity = quantity;
            VariantId = variantId;
        }

        public void IncreaseQuantity(int amount = 1)
        {
            Quantity += amount;
        }

        public void DecreaseQuantity(int amount = 1)
        {
            if (Quantity - amount < 0)
            {
                throw new InvalidOperationException("Quantity cannot be negative.");
            }
            Quantity -= amount;
        }

        public bool canBeDecreased(int amount = 1)
        {
            return Quantity - amount > 0;
        }
    }
}
