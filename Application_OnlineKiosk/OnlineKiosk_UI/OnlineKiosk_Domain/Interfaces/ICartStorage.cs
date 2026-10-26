using OnlineKiosk_Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Domain.Interfaces
{
    public interface ICartStorage
    {
        List<CartItem> GetCart();
        void SaveCart(List<CartItem> items);
        void ClearCart();
    }
}
