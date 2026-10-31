using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineKiosk_Domain.Data;
using OnlineKiosk_Domain.Interfaces;
using System.Linq;

namespace OnlineKiosk_UI.Pages
{
    public class CartPageModel : PageModel
    {
        private readonly ICartService _cartService;

        public Cart Cart{ get; set; }

        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal Total => Subtotal - Discount;

        public CartPageModel(ICartService cartService)
        {
            _cartService = cartService;
            Cart = _cartService.GetCart();
            Subtotal = Cart.Items.Sum(p => p.SubTotal());
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostIncrease(int variantId)
        {
            _cartService.IncreaseQuantity(variantId, 1);
            return QuantityUpdateResult(variantId);
        }

        public IActionResult OnPostDecrease(int variantId)
        {

            if (_cartService.DecreaseQuantity(variantId, 1))
            {
                //ToDo: send message to user to delete item from cart.
                _cartService.RemoveItem(variantId);
            }

            return QuantityUpdateResult(variantId);
        }

        private JsonResult QuantityUpdateResult(int variantId)
        {
            var cart = _cartService.GetCart();
            var item = cart.Items.FirstOrDefault(item => item.VariantId == variantId);
            var subtotal = cart.Items.Sum(item => item.SubTotal());

            return new JsonResult(new
            {
                variantId,
                quantity = item?.Quantity ?? 0,
                itemSubtotal = item?.SubTotal() ?? 0,
                subtotal,
                total = subtotal - Discount,
                removed = item is null,
                isEmpty = cart.Items.Count == 0
            });
        }

        public IActionResult OnPostConfirm()
        {
            if (Cart.Items.Count == 0)
            {
                return RedirectToPage();
            }

            _cartService.CompleteOrder();

            return RedirectToPage();
        }

        public Product getProductByCartItem(CartItem cartItem)
        {
            return _cartService.getProductByCartItem(cartItem);
        }
    }
}