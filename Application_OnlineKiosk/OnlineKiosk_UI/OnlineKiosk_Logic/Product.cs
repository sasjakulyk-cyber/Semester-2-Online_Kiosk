namespace OnlineKiosk_Logic
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public List<string> Allergens { get; set; }

        public List<ProductVariant> Variants { get; set; }

        public ProductVariant? GetDefaultVariant()
        {
            return Variants.OrderBy(v => v.Price).FirstOrDefault();
        }
        public string DefaultPrice()
        {
            return GetDefaultVariant()?.Price.ToString("0.00") ?? "0.00";
        }

        public string DefaultVariantDescription()
        {
            return GetDefaultVariant()?.Description ?? "";
        }

        public Product(int productId, string name, string description, string imageURL)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            ImageURL = imageURL;
            Allergens = new List<string>();
            Variants = new List<ProductVariant>();
        }
        public void AddVariant(int variantId, string description, decimal price)
        {
            var variant = new ProductVariant(variantId, description, price, this);
            Variants.Add(variant);
        }
    }
}
