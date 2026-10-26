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
        public void DecreaseQuantity(int variantId, int quantity);
        public void RemoveItem(int variantId);
        public void SaveCart();
        public void CompleteOrder();
    }
}
