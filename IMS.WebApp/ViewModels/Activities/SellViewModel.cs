using IMS.CoreBusiness;
using IMS.WebApp.ViewModelsValidations.Activities;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels.Activities
{
    public class SellViewModel
    {
        [Required]
        public string SalesOrderNumber { get; set; } = string.Empty;
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(minimum:1, maximum: int.MaxValue, ErrorMessage = "Quantity has to be greater than or equal to 1")]
        [SellViewModel_EnsureEnoughProductQuantity]
        public int QuantityToSell { get; set; }
        [Required]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "Quantity has to be greater than or equal to 0")]
        public double UnitPrice { get; set; }
        public Product? Product { get; set; } = null;
    }
}
