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
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public InventoryTransactionRepository(IDbContextFactory<IMSContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            using var dbContext = await contextFactory.CreateDbContextAsync();
            var query = from invTransactions in dbContext.InventoryTransactions
                        join inv in dbContext.Inventories
                        on invTransactions.InventoryId equals inv.InventoryId
                        where
                        (string.IsNullOrEmpty(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                        &&
                        (!dateFrom.HasValue || invTransactions.TransactionDate >= dateFrom.Value.Date)
                        &&
                        (!dateTo.HasValue || invTransactions.TransactionDate <= dateTo.Value.Date)
                        &&
                        (!transactionType.HasValue || invTransactions.ActivityType == transactionType.Value)
                        select invTransactions;
            return await query.Include(x => x.Inventory).ToListAsync();
        }

        public async Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
        {
            using var dbContext = await contextFactory.CreateDbContextAsync();
            dbContext.InventoryTransactions.Add(new()
            {
                ProductionNumber = productionNumber,
                InventoryId = inventory.InventoryId,
                ActivityType = InventoryTransactionType.ProduceProduct,
                QuantityBefore = inventory.Quantity,
                QuantityAfter = inventory.Quantity - quantityToConsume,
                UnitPrice = price,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy
            });
            await dbContext.SaveChangesAsync();
        }

        public async Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
        {
            using var dbContext = await contextFactory.CreateDbContextAsync();
            dbContext.InventoryTransactions.Add(new()
            {
                PoNumber = poNumber,
                InventoryId = inventory.InventoryId,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                QuantityBefore = inventory.Quantity,
                QuantityAfter = inventory.Quantity + quantity,
                UnitPrice = price,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy
            });
            await dbContext.SaveChangesAsync();
        }
    }
}
