﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private List<ProductTransaction> _productTransactions = new();

        private readonly IProductRepository productRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;

        public ProductTransactionRepository(IProductRepository productRepository, IInventoryTransactionRepository inventoryTransactionRepository, IInventoryRepository inventoryRepository)
        {
            this.productRepository = productRepository;
            this.inventoryTransactionRepository = inventoryTransactionRepository;
            this.inventoryRepository = inventoryRepository;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            var prod = await productRepository.GetProductByIdAsync(product.ProductId);
            if (prod != null) 
            {
                foreach(var productInventory in prod.ProductInventories) 
                {
                    if (productInventory.Inventory != null)
                        await inventoryTransactionRepository
                            .ProduceAsync(productionNumber, productInventory.Inventory, productInventory.InventoryQuantity * quantity, doneBy, -1);

                    var inv = await inventoryRepository.GetInventoryByIdAsync(productInventory.InventoryId);
                    inv.Quantity -= productInventory.InventoryQuantity * quantity;
                    await inventoryRepository.UpdateInventoryAsync(inv);
                }
            }

            _productTransactions.Add(new()
            {
                ProductionNumber = productionNumber,
                ProductId= product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity + quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy
            });


        }

        public Task SellProductAsync(string salesOrderNumber, Product product, int quantity,  double unitPrice, string doneBy)
        {
            _productTransactions.Add(new()
            {
                ActivityType= ProductTransactionType.SellProduct,
                SoNumber = salesOrderNumber,
                ProductId= product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
                UnitPrice = unitPrice
            });
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            var products = (await productRepository.GetProductsByNameAsync(string.Empty)).ToList();

            var query = from prodTransaction in _productTransactions
                        join prod in products
                        on prodTransaction.ProductId equals prod.ProductId
                        where
                        (string.IsNullOrEmpty(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                        &&
                        (!dateFrom.HasValue || prodTransaction.TransactionDate >= dateFrom.Value.Date)
                        &&
                        (!dateTo.HasValue || prodTransaction.TransactionDate <= dateTo.Value.Date)
                        &&
                        (!transactionType.HasValue || prodTransaction.ActivityType == transactionType.Value)
                        select new ProductTransaction
                        {
                            Product = prod,
                            ProductTransactionId = prodTransaction.ProductTransactionId,
                            SoNumber = prodTransaction.SoNumber,
                            ProductionNumber = prodTransaction.ProductionNumber,
                            ProductId = prodTransaction.ProductId,
                            ActivityType = prodTransaction.ActivityType,
                            QuantityBefore = prodTransaction.QuantityBefore,
                            QuantityAfter = prodTransaction.QuantityAfter,
                            UnitPrice = prodTransaction.UnitPrice,
                            TransactionDate = prodTransaction.TransactionDate,
                            DoneBy = prodTransaction.DoneBy
                        };
            return query;
        }
    }
}
