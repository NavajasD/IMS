using IMS.CoreBusiness;

namespace IMS.UseCases.UseCases.Products.Interfaces
{
    public interface IViewProductByIdUseCase
    {
        Task<Product> ExecuteAsync(int productId);
    }
}