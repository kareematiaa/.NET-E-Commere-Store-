using amazon.APIs.Dtos;
using amazon.APIs.errors;
using Amazon.Core.Entities.Order_Aggregate;
using Amazon.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace amazon.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]    
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(
            IOrderService orderService,
            IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreatOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var shippingAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

           var order = await _orderService.CreatOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, shippingAddress);

            if (order is null) 
                return BadRequest(new ApiResponse(400,"order is empty"));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>> (orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>>  GetOrderForUserById(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
             
            var order = await _orderService.GetOrderByIdForUserAsync(email,id);

            if (order is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Order, OrderToReturnDto> (order));
        }


        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDilveryMethod()
        {
            var deliveryMethod = await _orderService.GetDeliveryMethodsAsync();

            return Ok(deliveryMethod);
        }
    }
}
