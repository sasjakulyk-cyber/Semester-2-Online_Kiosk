using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineKiosk_Domain.Data;

namespace OnlineKiosk_Logic
{
    public class ProductModel
    {
        public Product Product { get; private set; }

        public ProductModel(Product product)
        {
            Product = product;
        }

        public ProductVariant? GetDefaultVariant()
        {
            return Product.Variants.OrderBy(v => v.Price).FirstOrDefault();
        }
        public string DefaultPrice()
        {
            return GetDefaultVariant()?.Price.ToString("0.00") ?? "0.00";
        }   

        public string DefaultVariantDescription()
        {
            return GetDefaultVariant()?.Description ?? "";
        }
    }
}
