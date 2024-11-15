using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productsCollection)
        {
            bool checkProducts = productsCollection.Find(p => true).Any();
            //string path = Path.Combine("Data", "SeedData", "products.json");
            if (!checkProducts)
            {
                //var productData = File.ReadAllText(path);
                var productData = File.ReadAllText("../Catalog.Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products != null)
                {
                    foreach (var product in products)
                    {
                        productsCollection.InsertOneAsync(product);
                    }
                }
            }
        }
    }
}
