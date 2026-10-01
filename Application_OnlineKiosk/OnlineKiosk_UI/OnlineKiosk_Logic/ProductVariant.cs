using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineKiosk_Logic
{
    public class ProductVariant
    {
        public int VariantId { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }
        public Product product { get; private set; }

        public ProductVariant(int variantId, string description, double price, Product product)
        {
            VariantId = variantId;
            Description = description;
            Price = price;
            this.product = product;
        }
    }
}
