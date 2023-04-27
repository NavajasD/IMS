using IMS.CoreBusiness;

namespace IMS.UseCases.UseCases.Products.Interfaces
{
    public interface IAddProductUseCase
    {
        Task ExecuteAsync(Product product);
    }
}