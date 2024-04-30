using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Error;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.APIs.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductCategory> _productCategoriesRepository;
        private readonly IMapper _mapper;
        public ProductController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductCategory> productCategoriesRepository, IMapper mapper)
        {
            _productCategoriesRepository = productCategoriesRepository;
            _productBrandRepository = productBrandRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAll([FromQuery] ProductSpecParams productSpecParams) //[FromQuery] to get the parameters from the query string
        {
            var spec = new ProductWithBrandAndCategorySpecifications(productSpecParams);
            var products = await _productRepository.GetAllAsyncSpec(spec);

            var countSpec = new ProductWithFiltersForCountSpecifications(productSpecParams);
            var totalItems = await _productRepository.GetCountWithSpecAsync(countSpec);
            // instead of return Ok(products); you can use AutoMapper to map the products to ProductDto
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            Pagination<ProductToReturnDto> pagination = new Pagination<ProductToReturnDto>(productSpecParams.PageIndex, productSpecParams.PageSize, totalItems, data);
            return Ok(pagination);
            
        }

        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)] // to show the response type in swagger
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]  // to show the response type in swagger

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetById(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            var product = await _productRepository.GetByIdAsyncSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));
            // instead of return Ok(product); you can use AutoMapper to map the product to ProductDto
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
            
        }

        [HttpGet("brands")]
        
        public async Task<ActionResult<ProductBrand>> GetAllBrands()
        {
            var Brands = await _productBrandRepository.GetAllAsync();
            return Ok(Brands);
        }

        [HttpGet("Categories")]
        public async Task<ActionResult<ProductCategory>> GetAllCategories()
        {
            var Categories = await _productCategoriesRepository.GetAllAsync();
            return Ok(Categories);
        }



        #region For Later

        //[HttpPost]
        //public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        //{
        //    var product = _mapper.Map<Product>(productDto);
        //    await _productService.AddProduct(product);
        //    return Ok();
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        //{
        //    var product = _mapper.Map<Product>(productDto);
        //    await _productService.UpdateProduct(id, product);
        //    return Ok();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    await _productService.DeleteProduct(id);
        //    return Ok();
        //} 
        #endregion
    }
}
