using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineKiosk_Logic;

namespace OnlineKiosk_UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProductService _productService;

        public IndexModel(ILogger<IndexModel> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public void OnGet()
        {
            _productService.GetCategories().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var categories = task.Result;
                    // Do something with the categories, e.g., store them in a property for the view
                }
                else
                {
                    _logger.LogError(task.Exception, "Error retrieving categories");
                }
            });
        }
    }
}
