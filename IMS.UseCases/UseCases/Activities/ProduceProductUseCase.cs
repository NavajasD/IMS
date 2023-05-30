using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.UseCases.Activities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.UseCases.Activities
{
    public class ProduceProductUseCase : IProduceProductUseCase
    {
        private readonly IProductTransactionRepository productTransactionRepository;
        private readonly IProductRepository productRepository;

        public ProduceProductUseCase(IProductTransactionRepository productTransactionRepository, IProductRepository productRepository)
        {
            this.productTransactionRepository = productTransactionRepository;
            this.productRepository = productRepository;
        }

        public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            //add a tramsaction record
            await productTransactionRepository.ProduceAsync(productionNumber, product, quantity, doneBy);

            //decrease the quantity inventories

            //update the quantity of products
            product.Quantity += quantity;
            await productRepository.UpdateProductAsync(product);
        }
    }
}
