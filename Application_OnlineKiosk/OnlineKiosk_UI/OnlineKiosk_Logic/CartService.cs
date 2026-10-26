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
        public Cart _cart{ get; private set; }

        public CartService(ICartRepository cartRepository, ICartStorage cartStorage)
        {
            _cartRepository = cartRepository;
            _cartStorage = cartStorage;
            _cart = new Cart(_cartStorage.GetCart());
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
                    var newItem = new CartItem(0, variantId, quantity);
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

        public void DecreaseQuantity(int variantId, int quantity = 1)
        {
            var itemToRemove = _cart.Items.FirstOrDefault(item => item.VariantId == variantId);
            if (itemToRemove != null && itemToRemove.Quantity <= quantity)
            {
                itemToRemove.DecreaseQuantity(quantity);
                // ToDo: Check if we need to save cart after cart change or SaveCart() should be called in the UI after all changes are made.
                SaveCart();
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
            _cartRepository.StoreCart(_cart);
            _cartStorage.ClearCart();
        }
    }
}
