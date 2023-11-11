using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repostory;
using TalabatAppAPIs.Dtos;
using TalabatAppAPIs.Erorrs;
using TalabatAppAPIs.Helpers;
using Talabt.Core.Entities;
using Talabt.Core.Repositories;
using Talabt.Core.Specification;
using Talabt.Core.Specifications;

namespace TalabatAppAPIs.Controllers
{
 
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _BrandRepo;
        private readonly IGenericRepository<ProductType> _TypeRepo;
        private readonly IMapper _maper;

        public ProductsController(IGenericRepository<Product>productsRepo, IGenericRepository<ProductBrand> BrandRepo, IGenericRepository<ProductType> TypeRepo, IMapper maper)
        {
            _productsRepo=productsRepo;
            _BrandRepo=BrandRepo;
            _TypeRepo=TypeRepo;
            _maper = maper;
        }
         [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //=>كتبناها بطريقة تانية في كلاس idenetytyserviceExtention
        //[Authorize] //دي بدل اللي فوق
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecPrams specPramas)
        {
            var spec =new ProductWithTypeAndBrand(specPramas);
            var products = await _productsRepo.GetAllWithSpecAsync(spec);
            //var result=new OkObjectResult(products);
            //return result;
            var countSpec = new countAllProductAfterFiltaraion(specPramas);
            var count=await _productsRepo.GetCountWithSpecAsync(countSpec);
            var data = _maper.Map<IReadOnlyCollection<Product>, IReadOnlyCollection <ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(specPramas.PageIndex,specPramas.PageSize,count,data));//IEnumerable<ProductToReturnDto>
        }

        //the next tow row show the propale result at swagger (Look at seager to remmb)
        [ProducesResponseType(typeof(ProductToReturnDto) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
           
            var spec = new ProductWithTypeAndBrand(id);
            var product = await _productsRepo.GetByIdWithSpecAsync(spec);

            if(product is null) return NotFound(new ApiResponse(404));
            //var products = await _productsRepo.GetByIdAsync(id);
            //var result=new OkObjectResult(products);
            //return result;
            return Ok(_maper.Map<Product,ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetBrands(int id)
        {
            var brands = await _BrandRepo.GetAllAsync();

            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<ProductBrand>> GetTypes(int id)
        {
            var types = await _TypeRepo.GetAllAsync();

            return Ok(types);
        }
    }
}
