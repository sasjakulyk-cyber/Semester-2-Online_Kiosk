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

        public Product(int productId, string name, string description, string imageURL)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            ImageURL = imageURL;
            Allergens = new List<string>();
            Variants = new List<ProductVariant>();
        }
        public void AddVariant(int variantId, string description, double price)
        {
            var variant = new ProductVariant(variantId, description, price, this);
            Variants.Add(variant);
        }
    }
}
