using IMS.WebApp.ViewModels.Activities;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelsValidations.Activities
{
    public class SellViewModel_EnsureEnoughProductQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var sellViewModel = validationContext.ObjectInstance as SellViewModel;
            if (sellViewModel != null) 
            {
                if (sellViewModel.Product != null) 
                {
                    if (sellViewModel.Product.Quantity < sellViewModel.QuantityToSell) 
                    {
                        return new ValidationResult($"There aren't enough products in inventory. There are currently {sellViewModel.Product.Quantity} items in inventory", 
                            new[] { validationContext.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
