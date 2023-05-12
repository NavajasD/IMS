﻿using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels.Activities
{
    public class ProduceViewModel
    {
        [Required]
        public string ProductionNumber { get; set; } = string.Empty;
        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "You need to select a product")]
        public int ProductId { get; set; }
        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int QuantityToProduce { get; set; }
    }
}
