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
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private readonly IMSContext dbContext;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IProductRepository productRepository;
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;

        public ProductTransactionRepository(IMSContext dbContext, IInventoryRepository inventoryRepository, IProductRepository productRepository, IInventoryTransactionRepository inventoryTransactionRepository)
        {
            this.dbContext = dbContext;
            this.inventoryRepository = inventoryRepository;
            this.productRepository = productRepository;
            this.inventoryTransactionRepository = inventoryTransactionRepository;
        }
        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            var query = from prodTransaction in dbContext.ProductTransactions
                        join prod in dbContext.Products
                        on prodTransaction.ProductId equals prod.ProductId
                        where
                        (string.IsNullOrEmpty(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                        &&
                        (!dateFrom.HasValue || prodTransaction.TransactionDate >= dateFrom.Value.Date)
                        &&
                        (!dateTo.HasValue || prodTransaction.TransactionDate <= dateTo.Value.Date)
                        &&
                        (!transactionType.HasValue || prodTransaction.ActivityType == transactionType.Value)
                        select prodTransaction;
            return await query.Include(x => x.Product).ToListAsync();
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            var prod = await productRepository.GetProductByIdAsync(product.ProductId);
            if (prod != null)
            {
                foreach (var productInventory in prod.ProductInventories)
                {
                    if (productInventory.Inventory != null)
                        await inventoryTransactionRepository
                            .ProduceAsync(productionNumber, productInventory.Inventory, productInventory.InventoryQuantity * quantity, doneBy, -1);

                    var inv = await inventoryRepository.GetInventoryByIdAsync(productInventory.InventoryId);
                    inv.Quantity -= productInventory.InventoryQuantity * quantity;
                    await inventoryRepository.UpdateInventoryAsync(inv);
                }
            }

            dbContext.ProductTransactions.Add(new()
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity + quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy
            });

            await dbContext.SaveChangesAsync();
        }

        public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            dbContext.ProductTransactions.Add(new()
            {
                ActivityType = ProductTransactionType.SellProduct,
                SoNumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
                UnitPrice = unitPrice
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
