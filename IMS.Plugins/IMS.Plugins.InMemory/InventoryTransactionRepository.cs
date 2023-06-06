using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly IInventoryRepository inventoryRepository;
        public List<InventoryTransaction> _inventoryTransactions = new();

        public InventoryTransactionRepository(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            var inventories = (await inventoryRepository.GetInventoriesByNameAsync(string.Empty)).ToList();

            var query = from invTransactions in _inventoryTransactions
                        join inv in inventories
                        on invTransactions.InventoryId equals inv.InventoryId
                        where
                        (string.IsNullOrEmpty(inventoryName) || inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                        &&
                        (!dateFrom.HasValue || invTransactions.TransactionDate >= dateFrom.Value.Date)
                        &&
                        (!dateTo.HasValue || invTransactions.TransactionDate <= dateTo.Value.Date)
                        &&
                        (!transactionType.HasValue || invTransactions.ActivityType == transactionType.Value)
                        select new InventoryTransaction
                        {
                            Inventory = inv,
                            InventoryTransactionId = invTransactions.InventoryTransactionId,
                            PoNumber = invTransactions.PoNumber,
                            InventoryId = invTransactions.InventoryId,
                            QuantityBefore= invTransactions.QuantityBefore,
                            ActivityType = invTransactions.ActivityType,
                            QuantityAfter= invTransactions.QuantityAfter,
                            TransactionDate= invTransactions.TransactionDate,
                            DoneBy = invTransactions.DoneBy,
                            UnitPrice = invTransactions.UnitPrice
                        };
            return query;
        }

        public Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
        {
            _inventoryTransactions.Add(new()
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
            return Task.CompletedTask;
        }

        public Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
        {
            _inventoryTransactions.Add(new()
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
            return Task.CompletedTask;
        }
    }
}
