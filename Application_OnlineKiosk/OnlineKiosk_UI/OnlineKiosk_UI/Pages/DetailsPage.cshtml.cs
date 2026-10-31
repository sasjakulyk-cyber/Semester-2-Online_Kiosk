using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineKiosk_Domain.Interfaces;
using OnlineKiosk_Domain.Data;
using OnlineKiosk_Logic.Models;

namespace OnlineKiosk_UI.Pages
{
    public class DetailsPageModel : PageModel
    {
        private readonly ILogger<DetailsPageModel> _logger;
        private readonly IProductService _productservice;
        private readonly ICartService _cartService;

        public List<Category> Categories { get; set; } = new List<Category>();
        public ProductModel ProductInfo { get; set; }

        public DetailsPageModel(ILogger<DetailsPageModel> logger, IProductService productservice, ICartService cartService)
        {
            _logger = logger;
            _productservice = productservice;
            _cartService = cartService;
        }
        public IActionResult OnGet(int id)
        {
            Categories = _productservice.GetCategories();

            var product = _productservice.GetProductDetailsByProductId(id);
            if (product != null)
            {
                ProductInfo = new ProductModel(product);
            }
            else
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                // Handle the case where the product is not found
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public IActionResult OnPostAddToCart(int variantId, int quantity)
        {
            if (quantity <= 0)
            {
                _logger.LogWarning($"Invalid quantity {quantity} for variant ID {variantId}.");
                // Handle invalid quantity
                return RedirectToPage();
            }
            if (variantId <= 0)
            {
                _logger.LogWarning($"Invalid variant ID {variantId}.");
                // Handle invalid quantity
                return RedirectToPage();
            }
            _cartService.IncreaseQuantity(variantId, quantity);
            return RedirectToPage("/Index");
        }
    }
}
