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
    public class ProductTransactionEntityTypeConfiguration : IEntityTypeConfiguration<ProductTransaction>
    {
        public void Configure(EntityTypeBuilder<ProductTransaction> builder)
        {
            builder
                .HasKey(x => x.ProductTransactionId);

            builder
                .HasOne(x => x.Product)
                .WithMany(x => x.ProductTransactions)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
