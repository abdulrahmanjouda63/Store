using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        // endPoint : public non-static method
        [HttpGet]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductResultDto>))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [Cache(100)]
        public async Task<ActionResult<PaginationResponse<ProductResultDto>>> GetAllProducts([FromQuery]ProductSpecificationsParameters specParameters)
        {
            var result = await serviceManager.ProductService.GetAllProductsAsync(specParameters);
            return Ok(result); // 200

        }

        [HttpGet("{id:int}")]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status200OK, Type = typeof(ProductResultDto))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ProductResultDto>> GetProductById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null) return NotFound(); // 404
            return Ok(result); // 200
        }

        [HttpGet("brands")]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandResultDto>))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ProductResultDto>> GetAllBrands()
        {
            var result = await serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest(); // 400
            return Ok(result); // 200
        }
        [HttpGet("types")]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypeResultDto>))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType<PaginationResponse<ProductResultDto>>(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ProductResultDto>> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest(); // 400
            return Ok(result); // 200
        }
    }
}
