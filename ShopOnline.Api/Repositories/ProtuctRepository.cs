using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categorias = await this.shopOnlineDbContext.ProductCategories.ToListAsync();
            return categorias;
        }

        public Task<ProductCategory> GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            if (this.shopOnlineDbContext == null)
            {
                throw new ArgumentNullException(nameof(this.shopOnlineDbContext), "DbContext is null.");
            }

            var products = await this.shopOnlineDbContext.Products.ToListAsync();
            return products;
        }
    }
}
