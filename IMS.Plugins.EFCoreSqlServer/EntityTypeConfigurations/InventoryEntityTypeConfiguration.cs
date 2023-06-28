using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer.EntityTypeConfigurations
{
    public class InventoryEntityTypeConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder
                .HasKey(x => x.InventoryId);

            builder
                .HasMany(x => x.ProductInventories)
                .WithOne(x => x.Inventory)
                .HasForeignKey(x => x.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.InventoryTransactions)
                .WithOne(x => x.Inventory)
                .HasForeignKey(x => x.InventoryId);
        }
    }
}
