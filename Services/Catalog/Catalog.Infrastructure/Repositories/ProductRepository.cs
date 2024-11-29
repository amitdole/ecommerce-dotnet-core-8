using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
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

        async Task<Pagination<Product>> IProductRepository.GetProducts(CatalogSpecParams catalogSpecParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            {
                filter = filter & builder.Where(p => p.Name.Contains(catalogSpecParams.Search, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
            {
                filter = filter & builder.Eq(p => p.Brands.Id, catalogSpecParams.BrandId);
            }

            if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
            {
                filter = filter & builder.Eq(p => p.Types.Id, catalogSpecParams.TypeId);
            }

            var totalItems = await Context.Products.Find(filter).CountDocumentsAsync();
            var data = await DataFilter(catalogSpecParams, filter);
          
            return new Pagination<Product>(
                catalogSpecParams.PageIndex, 
                catalogSpecParams.PageSize, 
                (int)totalItems, 
                data);
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

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            var sortData = Builders<Product>.Sort.Ascending(p => p.Name);

            if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            {
                switch (catalogSpecParams.Sort)
                {
                    case "priceAsc":
                        sortData = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "priceDesc":
                        sortData = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortData = Builders<Product>.Sort.Ascending(p => p.Name);
                        break;
                }
            }

            return new List<Product>(await Context.Products.Find(filter)
                .Sort(sortData)
                .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync());
        }
    }
}
