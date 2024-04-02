using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extension;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

                    //Mu Mehtot bize butun Productlari veriyor 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await this.productRepository.GetItems();
                var productCategories = await this.productRepository.GetCategories();

                if (products == null || productCategories == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productCategories);
                    return Ok(productDtos);
                }
            }
            catch (Exception ex)
            {
                // Hata günlüğüne kaydedin
                // Log the error
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving data from the database.");
            }
        }

        
                    //Mu Mehtot birtane
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await this.productRepository.GetItem(id);
               

                if (product == null )
                {
                    return BadRequest();
                }
                else
                {
                    var productCategory = await this.productRepository.GetCategory(product.CategoryId);
                    var productDTO= product.ConvertToDto(productCategory);
                    return Ok(productDTO);
                }
            }
            catch (Exception ex)
            {
                // Hata günlüğüne kaydedin
                // Log the error
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving data from the database.");
            }
        }


    }
}
