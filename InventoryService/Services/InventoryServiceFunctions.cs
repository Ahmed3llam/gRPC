using Grpc.Core;
using InventoryService.Proto;
using System.Collections.Concurrent;
using static InventoryService.Proto.Inventory;

namespace InventoryService.Services
{
    public class InventoryServiceFunctions : InventoryBase
    {
        private static readonly List<Product> Products = new List<Product>();
        //private static readonly ConcurrentDictionary<Int32, Product> Products = new ConcurrentDictionary<Int32, Product>();

        public override Task<ProductExistenceResponse> GetProductById(ProductIdRequest request, ServerCallContext context)
        {
            bool exists = Products.Any(p => p.Id == request.Id);
            return Task.FromResult(new ProductExistenceResponse { Exists = exists });
        }

        public override Task<AddProductResponse> AddProduct(Product request, ServerCallContext context)
        {
            Products.Add(request);
            return Task.FromResult(new AddProductResponse { Message = "Product added successfully" });
        }

        public override Task<UpdateProductResponse> UpdateProduct(Product request, ServerCallContext context)
        {
            var product = Products.FirstOrDefault(p => p.Id == request.Id);
            if (product != null)
            {
                product.Title = request.Title;
                product.Price = request.Price;
                product.Quantity = request.Quantity;
                return Task.FromResult(new UpdateProductResponse { Message = "Product updated successfully" });
            }
            else
            {
                return Task.FromResult(new UpdateProductResponse { Message = "Product not found" });
            }
        }
    }
}
