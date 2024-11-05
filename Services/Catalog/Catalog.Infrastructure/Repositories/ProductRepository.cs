using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
    {
        public ICatalogContext Context { get; }

        public ProductRepository(ICatalogContext context)
        {
            Context = context;
        }

        async Task<Product> IProductRepository.CreateProduct(Product product)
        {
            await Context.Products.InsertOneAsync(product);

            return product;
        }

        async Task<bool> IProductRepository.DeleteProduct(string id)
        {
            var deletedProduct = await Context.Products.DeleteOneAsync(p => p.Id == id);

            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }

        async Task<IEnumerable<ProductBrand>> IBrandRepository.GetAllBrands()
        {
            return await Context.Brands.Find(b => true).ToListAsync();
        }

        async Task<IEnumerable<ProductType>> ITypesRepository.GetAllTypes()
        {
            return await Context.Types.Find(t => true).ToListAsync();
        }

        async Task<Product> IProductRepository.GetProduct(string id)
        {
            return await Context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        async Task<IEnumerable<Product>> IProductRepository.GetProducts()
        {
            return await Context.Products.Find(p => true).ToListAsync();
        }

        async Task<IEnumerable<Product>> IProductRepository.GetProductsByBrand(string name)
        {
            return await Context.Products.Find(p => p.Brands.Name.ToLower() == name.ToLower()).ToListAsync();
        }

        async Task<IEnumerable<Product>> IProductRepository.GetProductsByName(string name)
        {
            return await Context.Products.Find(p => p.Name.ToLower() == name.ToLower()).ToListAsync();
        }

        async Task<bool> IProductRepository.UpdateProduct(Product product)
        {
            var updatedProduct = await Context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);

            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }
    }
}
