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
    public class ProductInventoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductInventory>
    {
        public void Configure(EntityTypeBuilder<ProductInventory> builder)
        {
            builder
                .HasKey(x => new {x.InventoryId, x.ProductId});

            builder
                .HasOne(x => x.Product)
                .WithMany(x => x.ProductInventories)
                .HasForeignKey(x => x.ProductId);

            builder
                .HasOne(x => x.Inventory)
                .WithMany(x => x.ProductInventories)
                .HasForeignKey(x => x.InventoryId);

        }
    }
}
