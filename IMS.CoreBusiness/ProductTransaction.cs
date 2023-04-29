using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class ProductTransaction
    {
        public int ProductTransactionId { get; set; }
        public string SoNumber { get; set; } = string.Empty;
        public string ProductionNumber { get; set; } = string.Empty;
        [Required]
        public int ProductId { get; set; }
        [Required]
        public ProductTransactionType ActivityType { get; set; }
        [Required]
        public int QuantityBefore { get; set; }
        [Required]
        public int QuantityAfter { get; set; }
        public double? UnitPrice { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        public string DoneBy { get; set; } = string.Empty;

        public Product? Inventory { get; set; }
    }
}
