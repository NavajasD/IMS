using IMS.CoreBusiness;

namespace IMS.UseCases.UseCases.Products.Interfaces
{
    public interface IEditProductUseCase
    {
        Task ExecuteAsync(Product product);
    }
}