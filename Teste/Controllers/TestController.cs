using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Teste.Models;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Teste.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public string Get()
        {
            return "Fullstack Challenge 20201026";
        }

        // GET: Products
        [HttpGet("products")]
        public IEnumerable<Product> GetProducts()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetValue<string>("ConnectionMongo"));
            var db2 = dbClient.GetDatabase("teste_api");
            var coll = db2.GetCollection<ProductToList>("products").Aggregate().ToList();
            List<Product> products = new List<Product>();
            foreach (var item in coll)
            { 
                products.Add(item);
            }

            DateTime now = DateTime.Now;
            TimeSpan timeSpan = DateTime.Now - Connect.Last;
           
            if (timeSpan.Hours > 24)
            {
                Connect.Start(_configuration, true);
            }

            return products;
        }

        // GET Products/5
        [HttpGet("products/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(long code)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetValue<string>("ConnectionMongo"));
            var db2 = dbClient.GetDatabase("teste_api");
            var coll = db2.GetCollection<ProductToList>("products").Aggregate().ToList();

            var returnValue = coll.FirstOrDefault(x => x.cod == code);

            DateTime now = DateTime.Now;
            TimeSpan timeSpan = now - Connect.Last;
            
            if (timeSpan.Hours > 24)
            {
                Connect.Start(_configuration, true);
            }

            if (returnValue != null)
            {
                Product p = new Product(returnValue);
                return Ok(p);
            }

            return BadRequest();
        }
    }
}
