using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/Products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider _productsProvider;
        public ProductsController(IProductsProvider productsProvider)
        {
            _productsProvider = productsProvider;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await _productsProvider.GetProductsAsync();
            if (result.Isuccess)
            {
                return Ok(result.Products);
            }
            return NotFound();
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetProductAsync(int ID)
        {
            var result = await _productsProvider.GetProductAsync(ID);
            if (result.Isuccess)
            {
                return Ok(result.Product);
            }
            return NotFound();
        }
    }
}
