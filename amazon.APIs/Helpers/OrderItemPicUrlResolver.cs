using amazon.APIs.Dtos;
using Amazon.Core.Entities.Order_Aggregate;
using AutoMapper;


namespace amazon.APIs.Helpers
{
    public class OrderItemPicUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {

        public  IConfiguration _configuration { get; }

        public OrderItemPicUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["ApiBsseUrl"]}{source.Product.PictureUrl}";


            return string.Empty;
        }
    }
}
