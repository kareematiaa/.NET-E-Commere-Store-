using amazon.APIs.Dtos;
using amazon.APIs.errors;
using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace amazon.APIs.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet] 
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {

            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var updatedOrCreatedBasket = await _basketRepository.UbdateBasketAsync(mappedBasket);
            if (updatedOrCreatedBasket is null)
                return BadRequest(new ApiResponse(400));
            return Ok(updatedOrCreatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasket(id); 
        }
    }
}
