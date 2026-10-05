using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineKiosk_Logic;

namespace OnlineKiosk_UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProductService _productService;

        [BindProperty(SupportsGet = true)]
        public int? ActiveCategoryId { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Product> Products { get; set; } = new List<Product>();

        public IndexModel(ILogger<IndexModel> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        //bool SelectIf(Category c)
        //{
        //    return (c.CategoryId == ActiveCategoryId);
        //}

        //Category FirstOrDefault(Iterable<Category> categories, Category defaultCategory)
        //{
        //    foreach(var c in categories)
        //    {
        //        if (SelectIf(c))
        //        {
        //            return c;
        //        }
        //    }
        //    return defaultCategory;
        //}

        public void OnGet()
        {
            Categories = _productService.GetCategories();
            if (Categories.Count > 0)
            {
                if (!ActiveCategoryId.HasValue)
                {
                    ActiveCategoryId = Categories[0].CategoryId;
                }

                Categories.FirstOrDefault(c => c.CategoryId == ActiveCategoryId, Categories[0]).IsActive = true;
            }
            else
            {
                _logger.LogWarning("No categories found.");
            }

            Products = _productService.GetProductsByCategory(ActiveCategoryId ?? Categories[0].CategoryId);
        }
    }
}
