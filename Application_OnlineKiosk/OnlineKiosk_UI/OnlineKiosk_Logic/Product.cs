namespace OnlineKiosk_Logic
{
    public class Product
    {
        public int ProductId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageURL { get; private set; }
        public List<string> Allergens { get; private set; }

        public List<ProductVariant> Variants { get; private set; }

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
