using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineKiosk_Domain.Interfaces;
using OnlineKiosk_Domain.Data;
using System.ComponentModel.Design;

namespace OnlineKiosk_Logic
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartStorage _cartStorage;
        private readonly Cart _cart;
        private readonly Dictionary<int, Product> _products = new Dictionary<int, Product>();

        public CartService(ICartRepository cartRepository, ICartStorage cartStorage)
        {
            _cartRepository = cartRepository;
            _cartStorage = cartStorage;
            _cart = new Cart(_cartStorage.GetCart());
            if (_cart.Items.Any())
            {
                _products = _cartRepository.GetProductsInCart(_cart.Items).ToDictionary(p => p.Variants[0].VariantId, p => p);
            }
        }

        public void IncreaseQuantity(int variantId, int quantity = 1)
        {
            var itemToRemove = _cart.Items.FirstOrDefault(item => item.VariantId == variantId);
            if (itemToRemove != null)
            {
                itemToRemove.IncreaseQuantity(quantity);
            }
            else
            {
                var productVariant = _cartRepository.GetProductVariantById(variantId);
                if (productVariant != null)
                {
                    var newItem = new CartItem(0, variantId, quantity, productVariant.Price);
                    _cart.Items.Add(newItem);
                }
                else
                {
                    throw new ArgumentException($"Product variant with ID {variantId} not found.");
                }
            }

            // ToDo: Check if we need to save cart after cart change or SaveCart() should be called in the UI after all changes are made.
            SaveCart();
        }

        public bool DecreaseQuantity(int variantId, int quantity = 1)
        {
            var itemToRemove = _cart.Items.FirstOrDefault(item => item.VariantId == variantId);
            if (itemToRemove != null)
            {
                itemToRemove.DecreaseQuantity(quantity);
                if (!itemToRemove.needToRemove()) // Need to remove from cart if quantity is 0 or less
                {
                    // ToDo: Check if we need to save cart after cart change or SaveCart() should be called in the UI after all changes are made.
                    SaveCart();
                    return false;
                }
                return true;
            }
            else
            {
                throw new ArgumentException($"Product variant with ID {variantId} not found in the cart.");
            }
        }

        public void RemoveItem(int variantId)
        {
            var itemToRemove = _cart.Items.FirstOrDefault(item => item.VariantId == variantId);
            if (itemToRemove != null)
            {
                _cart.Items.Remove(itemToRemove);
                // ToDo: Check if we need to save cart after cart change or SaveCart() should be called in the UI after all changes are made.
                SaveCart();
            }
            else
            {
                throw new ArgumentException($"Product variant with ID {variantId} not found in the cart.");
            }
        }

        public void SaveCart()
        {
            _cartStorage.SaveCart(_cart.Items);
        }

        public void CompleteOrder()
        {
            _cartRepository.CompleteOrder(_cart);
            _cartStorage.ClearCart();
        }

        public Product getProductByCartItem(CartItem cartItem)
        {
            if (_products.TryGetValue(cartItem.VariantId, out var product))
            {
                return product;
            }
            else
            {
                throw new ArgumentException($"Product for variant ID {cartItem.VariantId} not found.");
            }
        }

        public Cart GetCart()
        {
            return _cart;
        }

        public decimal GetSubtotal()
        {
            return _cart.Items.Sum(item => item.Price * item.Quantity);
        }

        public decimal GetTotal()
        {
            return GetSubtotal() /*- Discount*/;
        }
    }
}
