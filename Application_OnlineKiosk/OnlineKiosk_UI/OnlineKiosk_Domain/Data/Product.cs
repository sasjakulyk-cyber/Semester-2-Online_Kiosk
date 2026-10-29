namespace OnlineKiosk_Domain.Data
{
    public class Product
    {
        public int ProductId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageURL { get; private set; }
        public List<string> Allergens { get; private set; }

        public List<ProductVariant> Variants { get; private set; }

        public Product(int productId)
        {
            ProductId = productId;
            Name = "";
            Description = "";
            ImageURL = "";
            Allergens = new List<string>();
            Variants = new List<ProductVariant>();
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

        //ToDo: Add functions to access a sigle variant ( product.Variants[0] to become product.Variant() with verification of a single variant item)
    }
}
