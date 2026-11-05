using OnlineKiosk_Domain.Data;
using OnlineKiosk_Domain.Interfaces;

namespace OnlineKiosk_UnitTestLogic.Mocks
{
    internal class FakeCartRepository : ICartRepository
    {
        public ProductVariant? VariantToReturn { get; set; }
        public Dictionary<int, ProductVariant> Variants { get; } = new();
        public int CompleteOrder(Cart cart) => 1;
        public ProductVariant? GetProductVariantById(int id) =>
            //if (Variants.TryGetValue(id, out var variant))
            //{
            //    return variant;
            //}
            //else
            //{
            //    return VariantToReturn?.VariantId == id? VariantToReturn : null;
            //}

            Variants.TryGetValue(id, out var variant)
                ? variant
                : VariantToReturn?.VariantId == id ? VariantToReturn : null;
        public List<Product> GetProductsInCart(List<CartItem> cartItems) => new();
    }
}
