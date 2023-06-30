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
    public class InventoryTransactionEntityTypeConfiguration : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder
                .HasKey(x => x.InventoryTransactionId);
            builder
                .HasOne(x => x.Inventory)
                .WithMany(x => x.InventoryTransactions)
                .HasForeignKey(x => x.InventoryId);
        }
    }
}
