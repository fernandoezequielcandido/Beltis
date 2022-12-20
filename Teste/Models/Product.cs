using MongoDB.Bson;
using Newtonsoft.Json;

namespace Teste.Models
{
    public class Product
    {
        public Product()
        {

        }

        public Product(ProductToList p)
        {
            this.cod = p.cod;
            this.barcode = p.barcode;
            this.status = p.status;
            this.imported_t = p.imported_t;
            this.url = p.url;
            this.product_name = p.product_name;
            this.quantity = p.quantity;
            this.categories = p.categories;
            this.packaging = p.packaging;
            this.brands = p.brands;
            this.image_url = p.image_url;
        }
        public long cod { get; set; }
        public string barcode { get; set; } = string.Empty;
        public status status { get; set; }
        public DateTime imported_t { get; set; }
        public string url { get; set; } = string.Empty;
        public string product_name { get; set; } = string.Empty;
        public string quantity { get; set; } = string.Empty;
        public string categories { get; set; } = string.Empty;
        public string packaging { get; set; } = string.Empty;
        public string brands { get; set; } = string.Empty;
        public string image_url { get; set; } = string.Empty;
    }

    public enum status 
    {
        draft,
        imported
    }
}
