using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Domain.Data
{
    public class Cart
    {
        public int CartId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<CartItem> Items { get; private set; }

        public Cart(List<CartItem> items)
        {
            CartId = 0; // Default value, can be set later
            CreatedAt = DateTime.Now;
            Items = items ?? new List<CartItem>();
        }

        public Cart(int cartId, DateTime createdAt, List<CartItem> items)
        {
            CartId = cartId;
            CreatedAt = createdAt;
            Items = items;
        }

        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.Item.VariantId == item.Item.VariantId);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(item.Quantity);
            }
            else
            {
                Items.Add(item);
            }
        }
    }
}
