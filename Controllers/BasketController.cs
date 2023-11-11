using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatAppAPIs.Erorrs;
using Talabt.Core.Entities;
using Talabt.Core.Repositories;

namespace TalabatAppAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository= basketRepository;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            
            return basket ?? new CustomerBasket(id);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket BASKET)
        {
            var createdrUpdatedBasket = await _basketRepository.UpdateBasketAsync(BASKET);
            
            if(createdrUpdatedBasket is null) { return BadRequest(new ApiResponse(400)); }

            return createdrUpdatedBasket;
        }

        [HttpDelete ]
        public async Task<ActionResult  <bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBaseketAsync(id);
        }
    }
}
