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
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IMSContext dbContext;

        public InventoryRepository(IMSContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddInventoryAsync(Inventory inventory)
        {
            dbContext.Inventories.Add(inventory);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            return await dbContext
                .Inventories
                .Where(x => x.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            var inv = await dbContext.Inventories.FindAsync(inventoryId);
            if (inv != null) return inv;
            return new Inventory();
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            var inv = await dbContext.Inventories.FindAsync(inventory.InventoryId);
            if (inv != null) 
            {
                inv.InventoryName = inventory.InventoryName;
                inv.Price = inventory.Price;
                inv.Quantity = inventory.Quantity;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
