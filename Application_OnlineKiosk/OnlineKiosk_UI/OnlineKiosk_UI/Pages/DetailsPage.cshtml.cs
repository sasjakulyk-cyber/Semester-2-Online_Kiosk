using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineKiosk_Logic;

namespace OnlineKiosk_UI.Pages
{
    public class DetailsPageModel : PageModel
    {
        private readonly ILogger<DetailsPageModel> _logger;
        private readonly IProductService _productservice;

        public List<Category> Categories { get; set; } = new List<Category>();
        public Product ProductInfo { get; set; }

        public DetailsPageModel(ILogger<DetailsPageModel> logger, IProductService productservice)
        {
            _logger = logger;
            _productservice = productservice;
        }
        public IActionResult OnGet(int id)
        {
            Categories = _productservice.GetCategories();

            var product = _productservice.GetProductDetailsByProductId(id);
            if (product != null)
            {
                ProductInfo = product;
            }
            else
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                // Handle the case where the product is not found
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
