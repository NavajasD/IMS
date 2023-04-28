using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels.Activities
{
    public class PurchaseViewModel
    {
        [Required]
        public string PONumber { get; set; } = string.Empty;
        [Required]
        [Range(minimum: 1, maximum:int.MaxValue, ErrorMessage ="You need to select an inventory to purchase")]
        public int InventoryId { get; set; }
        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int QuantityToPurchase { get; set; }
        public double InventoryPrice { get; set; }
    }
}
