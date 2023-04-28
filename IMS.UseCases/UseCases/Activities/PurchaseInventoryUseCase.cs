using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.UseCases.Activities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.UseCases.Activities
{
    public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;

        public PurchaseInventoryUseCase(IInventoryRepository inventoryRepository, IInventoryTransactionRepository inventoryTransactionRepository)
        {
            this.inventoryRepository = inventoryRepository;
            this.inventoryTransactionRepository = inventoryTransactionRepository;
        }

        public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
        {
            //insert a record in the transaction table
            await inventoryTransactionRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

            //increase the quantity
            inventory.Quantity += quantity;
            await inventoryRepository.UpdateInventoryAsync(inventory);
        }
    }
}
