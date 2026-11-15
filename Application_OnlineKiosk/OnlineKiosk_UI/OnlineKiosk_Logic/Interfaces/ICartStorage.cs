using OnlineKiosk_Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Logic.Interfaces
{
    public interface ICartStorage
    {
        List<CartItem> GetCart();
        void SaveCart(List<CartItem> items);
        void ClearCart();
    }
}
