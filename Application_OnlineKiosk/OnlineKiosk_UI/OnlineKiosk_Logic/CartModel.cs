using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineKiosk_Domain.Data;

namespace OnlineKiosk_Logic
{
    public class CartModel
    {
        public Cart Cart { get; private set; }

        public CartModel()
        {
            Cart = new Cart();
        }
    }
}
