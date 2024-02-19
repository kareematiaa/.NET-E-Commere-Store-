using amazon.APIs.Dtos;
using amazon.APIs.errors;
using Amazon.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace amazon.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IpaymentService _paymentService;

        public PaymentsController(IpaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null)
                return BadRequest(new ApiResponse(400, "A Problem With Your Basket"));

            return Ok(basket);
        }

    }
}
