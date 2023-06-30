using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public ProductRepository(IDbContextFactory<IMSContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public async Task AddProductAsync(Product product)
        {
            using var dbContext = await contextFactory.CreateDbContextAsync();
            dbContext.Products.Add(product);
            FlagInventoryUnchanged(product, dbContext);
            await dbContext.SaveChangesAsync();
        }

        private void FlagInventoryUnchanged(Product product, IMSContext db)
        {
            if (product?.ProductInventories != null &&
                            product.ProductInventories.Count >= 0)
            {
                foreach (var item in product.ProductInventories)
                {
                    if (item.Inventory != null)
                        db.Entry(item.Inventory).State = EntityState.Unchanged;
                }
            }
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            using var dbContext = await contextFactory.CreateDbContextAsync();
            var product = await dbContext
                .Products
                .Include(x => x.ProductInventories)
                .ThenInclude(x => x.Inventory)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (product != null) return product;
            return new Product();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            using var dbContext = await contextFactory.CreateDbContextAsync();
            return await dbContext
                .Products
                .Where(x => x.ProductName.ToLower().IndexOf(name.ToLower()) >= 0)
                .ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            using var dbContext = await contextFactory.CreateDbContextAsync();
            var prod = await dbContext
                .Products
                .Include(x => x.ProductInventories)
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (prod != null) 
            {

                prod.ProductName = product.ProductName;
                prod.Price = product.Price;
                prod.Quantity = product.Quantity;
                prod.ProductInventories = product.ProductInventories;

                FlagInventoryUnchanged(prod, dbContext);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
