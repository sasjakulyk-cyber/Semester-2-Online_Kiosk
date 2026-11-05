using OnlineKiosk_Domain.Data;
using OnlineKiosk_Domain.Interfaces;

namespace OnlineKiosk_UnitTestLogic.Mocks
{
    public class FakeCartStorage : ICartStorage
    {
        private readonly List<CartItem> _items;
        public FakeCartStorage(params CartItem[] items)
        {
            Items = items.ToList();
        }
        public List<CartItem> Items { get; private set; } = new();
        public List<CartItem> GetCart() => Items;
        public void SaveCart(List<CartItem> items)
        {
            Items = items.ToList();
        }
        public void ClearCart() => Items.Clear();
    }
}
