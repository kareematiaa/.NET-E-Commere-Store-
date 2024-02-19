using amazon.APIs.Dtos;
using amazon.APIs.errors;
using amazon.APIs.Helpers;
using Amazon.Core;
using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using Amazon.Core.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace amazon.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        
        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<ProductBrand> _brandsRepo;
        ///private readonly IGenericRepository<ProductType> _typeRepo;

        public ProductsController(
            ///IGenericRepository<Product> productRepo,
            ///IGenericRepository<ProductBrand> brandsRepo,
            ///IGenericRepository<ProductType> typeRepo,
          
            IUnitOfWork unitOfWork,
            IMapper mapper
            ) 
        {
            ///_productRepo = productRepo;
            ///_brandsRepo = brandsRepo;
            ///_typeRepo = typeRepo;
           

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams SpecParams)
        {
            var spec = new ProductWithBrandAndTypeSpecification(SpecParams);

            var products =await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var countSpec = new ProductWithFilterationForCountSpecification(SpecParams);

            var count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(SpecParams.PageIndex, SpecParams.PageSize,count, data)); 
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);  
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            if (product is null)
                return NotFound(new ApiResponse(404));
            
           return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }



        [HttpGet("brands")] //Get : api/products/brands
         public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);

        }


        [HttpGet("types")] //Get : api/products/types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);

        }
        
    }
}
