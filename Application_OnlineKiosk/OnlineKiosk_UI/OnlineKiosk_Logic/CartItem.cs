using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Logic
{
    public class CartItem
    {
        public int CartItemId { get; private set; }
        public int Quantity { get; private set; }
        public ProductVariant Item { get; private set; }

    }
}
