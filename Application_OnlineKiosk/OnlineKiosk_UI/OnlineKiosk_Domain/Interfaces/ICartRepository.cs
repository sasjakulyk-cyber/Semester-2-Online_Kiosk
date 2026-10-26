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
        public int StoreCart(Cart cart);
    }
}
