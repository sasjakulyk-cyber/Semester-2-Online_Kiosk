using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Logic
{
    public class Cart
    {
        public int CartId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<CartItem> Items { get; private set; }
    }
}
