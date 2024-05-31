using Client.Models;
using Client.Proto;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddOrUpdateProduct(ProductModel product)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7042");
            var client = new Client.Proto.Inventory.InventoryClient(channel);
            var ExistRequest = new ProductIdRequest { Id = product.id };
            var ExistResponse = await client.GetProductByIdAsync(ExistRequest);
            if (ExistResponse != null) {
                var Request = new Product
                {
                    Id = product.id,
                    Title = product.title,
                    Price = product.price,
                    Quantity = product.quantity,
                };
                if (ExistResponse.Exists == true) {
                    var UpdateResponse = await client.UpdateProductAsync(Request);
                    return Ok(new { Status = 201, Product = Request , Msg=UpdateResponse.Message });
                }
                else { 
                    var InsertResponse = await client.AddProductAsync(Request);
                    return Created("",new { Status = 200, Product = Request , Msg = InsertResponse.Message });
                }
            }
            else {
                return NotFound(new { message = "Response For Product Equal Null" });
            }
        }

    }
}
