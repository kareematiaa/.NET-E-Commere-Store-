using amazon.APIs.errors;
using Amazon.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace amazon.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("notFound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(100);
            if (product == null) return NotFound(new ApiResponse(404));

            return Ok(product);
        }


        [HttpGet("serverError")]
        public ActionResult GetServerError()
        {
            var product = _dbContext.Products.Find(100);
            var productToReturn = product.ToString();

            return Ok(productToReturn);
        }


        [HttpGet("badRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }



        [HttpGet("badRequest/{id}")]
        public ActionResult GetBadRequestvalidation(int id)
        {
            return Ok();
        }
    }
}
