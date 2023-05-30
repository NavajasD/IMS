﻿using IMS.CoreBusiness;
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
        public List<InventoryTransaction> _inventoryTransactions = new();

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
