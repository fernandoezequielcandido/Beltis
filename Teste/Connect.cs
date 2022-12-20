using HtmlAgilityPack;
using MongoDB.Driver;
using Teste.Models;

namespace Teste
{
    public static class Connect
    {
        public static DateTime Last;
        private static HttpClient client = new HttpClient();
        public static async Task StartAsync(IConfiguration config, bool force = false)
        {
            MongoClient dbClient = new MongoClient(config.GetValue<string>("ConnectionMongo"));
            var db2 = dbClient.GetDatabase("teste_api");
            var listCount = db2.GetCollection<ProductToList>("products").Aggregate().ToList();

            if (listCount.Count == 0 || force)
            {
                string url = "https://world.openfoodfacts.org/";
                var response = await CallUrl(url);

                var products = await ParseHtml(response);


                var coll = db2.GetCollection<Product>("products");

                var cods = products.Select(x => x.cod).ToList();
                coll.DeleteMany(Builders<Product>.Filter.In("cod", cods));
                coll.InsertMany(products);
                Last = DateTime.Now;
            }
        }

        public static void Start(IConfiguration config, bool force = false)
        {
            MongoClient dbClient = new MongoClient(config.GetValue<string>("ConnectionMongo"));
            var db2 = dbClient.GetDatabase("teste_api");
            var listCount = db2.GetCollection<ProductToList>("products").Aggregate().ToList();

            if (listCount.Count == 0 || force)
            {
                string url = "https://world.openfoodfacts.org/";
                var response = CallUrl(url).Result;

                var products = ParseHtml(response).Result;

                var coll = db2.GetCollection<Product>("products");

                var cods = products.Select(x => x.cod).ToList();
                coll.DeleteMany(Builders<Product>.Filter.In("cod", cods));
                coll.InsertMany(products);
                Last = DateTime.Now;
            }
        }

        private static async Task<string> CallUrl(string fullUrl)
        {
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }

        private static async Task<List<Product>> ParseHtml(string html)
        {
            List<Product> result = new List<Product>();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var as_ = htmlDoc.DocumentNode.SelectNodes("//a");

            foreach (var a in as_)
            {
                if (a.GetAttributes("href").Count() > 0)
                { 
                    var primeiro = a.GetAttributes("href").First();
                    if (primeiro != null && primeiro.DeEntitizeValue.Contains("product"))
                    {
                        string url = "https://world.openfoodfacts.org/";
                        string[] partes = primeiro.DeEntitizeValue.Split("/");

                        if (partes.Length == 4)
                        {
                            url = url + partes[1] + "/" + partes[2];
                            var response = await CallUrl(url);
                            string imageUrl = await GetImageUrl(partes[2]);
                            ParseHtmlProduct(response, partes[2], url, imageUrl, result);
                            
                            if (result.Count == 100)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static void ParseHtmlProduct(string html, string code, string url, string imageUrl, List<Product> result)
        {
            Product product = new Product();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            product.cod = long.Parse(code);

            var barcode = htmlDoc.GetElementbyId("barcode_paragraph");
            if (barcode != null)
            {
                product.barcode = barcode.InnerText.Replace("Barcode:", "").Replace("\\n", "").TrimStart().TrimEnd();
            }
            
            foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//h2[@class='title-1']"))
            {
                product.product_name = node.InnerText.Split('-')[0].Trim();
                break;
            }
            
            var quantity = htmlDoc.GetElementbyId("field_quantity_value");
            if (quantity != null)
            {
                product.quantity = quantity.InnerText;
            }

            var packaging = htmlDoc.GetElementbyId("field_packaging_value");
            if (packaging != null)
            {
                product.packaging = packaging.InnerText;
            }

            var brands = htmlDoc.GetElementbyId("field_brands_value");
            if (brands != null)
            {
                product.brands = brands.InnerText;
            }

            var categories = htmlDoc.GetElementbyId("field_categories_value");
            if (categories != null)
            {
                product.categories = categories.InnerText;
            }

            product.imported_t = DateTime.Now;

            product.image_url = imageUrl;

            product.url = url;

            product.status = status.imported;
            
            result.Add(product);
        }

        private static async Task<string> GetImageUrl(string cod)
        {
            string barcodeImage = "https://static.openfoodfacts.org/images/products/";

            if (cod.Length > 8)
            {
                int iBarra = 0;
                for (int i = 0; i < cod.Length; i++)
                {
                    barcodeImage = barcodeImage + cod[i];
                    iBarra++;
                    if (iBarra == 3 && i < 9)
                    {
                        barcodeImage = barcodeImage + "/";
                        iBarra = 0;
                    }
                }
            }
            else
            {
                barcodeImage = barcodeImage + "/" + cod;
            }

            var json = await client.GetStringAsync("https://world.openfoodfacts.org/api/v0/product/" + cod + ".json");
            
            var firstSplit = json.Split(new string[] { "\"front_fr\"" }, StringSplitOptions.None);
            string nameFile = "front_fr";

            string number = "";

            if (firstSplit.Length < 2)
            {
                firstSplit = json.Split(new string[] { "\"front_en\"" }, StringSplitOptions.None);
                nameFile = "front_en";
            }

            if (firstSplit.Length < 2)
            {
                firstSplit = json.Split(new string[] { "\"front_es\"" }, StringSplitOptions.None);
                nameFile = "front_es";
            }

            if (firstSplit.Length > 1)
            {
                var secondSplit = firstSplit[1].Split(new string[] { "\"rev\":\"" }, StringSplitOptions.None);

                int iChar = 0;
                while (secondSplit[1][iChar] != '"')
                {
                    number = number + secondSplit[1][iChar];
                    iChar++;
                }
            }
            
            barcodeImage = barcodeImage + "/" + nameFile + "." + number + ".400.jpg";

            return barcodeImage;
        }
    }
}
