using Microsoft.AspNetCore.Mvc;

namespace OnlineStore_Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllProducts(int? maxLimit)
    {
        var products = await _productService.GetAllProductsAsync(maxLimit);
        return Ok(products);
    }
    [HttpGet("{productID}")]
    public async Task<IActionResult> GetProductById(int productID)
    {
        var products = await _productService.GetFullProductByIDAsync(productID);
        return Ok(products);
    }
}
