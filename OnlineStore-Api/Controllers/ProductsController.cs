using Mapster;
using Microsoft.AspNetCore.Mvc;
using OnlineStore_Api.Dtos.Product;
using OnlineStore_Api.Helpers;
using System.Linq;

namespace OnlineStore_Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ICateogryService _cateogryService;
    private readonly IImageService _imageService;

    public ProductsController(IProductService productService, ICateogryService cateogryService, IImageService imageService)
    {
        _productService = productService;
        _cateogryService = cateogryService;
        _imageService = imageService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllProducts(int? maxLimit)
    {
        var products = (await _productService.GetAllProductsAsync(maxLimit))
                             .Select(prod=>prod.Adapt<ProductDto>());
        return Ok(products);
    }
    [HttpGet]
    public async Task<IActionResult> GetProductsWithQuery(
        [FromQuery] int? categoryID,
        [FromQuery] string? nameQuery,
        [FromQuery] bool? includeImages,
        [FromQuery] int? page,
        [FromQuery] int? limit)
    {
        var searchFilter = new ProductSearchFilter
        {
            CategoryId = categoryID,
            Name = nameQuery,
            Page = page,
            Limit = limit,
            IncludeImages = includeImages
        };
        IEnumerable<Product>? products = await _productService.GetAllProductsAsync(searchFilter);
        IEnumerable<ProductDto>? productsDtos = products.Select(prod => prod.Adapt<ProductDto>());
        return Ok(products);
    }
    [HttpGet("{productID}")]
    public async Task<IActionResult> GetProductById(int productID)
    {
        var product = await _productService.GetFullProductByIDAsync(productID);
        if (product is null)
            return NotFound();

        return Ok(product.Adapt<ProductDto>());
    }
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromForm]AddProductDto addProductDto)
    {
        bool categoryExist = await _cateogryService.CheckCategoryExistAsync(addProductDto.CategoryId);
        if (!categoryExist)
            return BadRequest("category does not exist");

        var product = addProductDto.Adapt<Product>();
        var createdProduct = await _productService.AddNewProductAsync(product);

        // Sae images
        foreach (var imageDto in addProductDto.ProductImageDtos)
        {
            await _imageService.SaveImage(imageDto, createdProduct.Id);
        }

        return Ok(createdProduct);
    }
}
