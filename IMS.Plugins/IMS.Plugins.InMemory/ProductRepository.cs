using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;

        public ProductRepository()
        {
            _products = new()
            {
                new() {ProductId = 1, ProductName = "Bike", Quantity = 10, Price = 150},
                new() {ProductId = 2, ProductName = "Car", Quantity = 5, Price = 25000}
            };
        }
        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                return await Task.FromResult(_products);

            return _products.Where(x => x.ProductName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
