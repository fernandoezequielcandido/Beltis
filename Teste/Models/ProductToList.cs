using MongoDB.Bson;
using System.Runtime.Serialization;

namespace Teste.Models
{
    public class ProductToList: Product
    {
        public ObjectId id { get; set; }
    }
}
