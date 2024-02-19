using amazon.APIs.Helpers;
using Amazon.Core;
using Amazon.Core.Repositories;
using Amazon.Core.Services;
using Amazon.Repository;
using Amazon.Service;
using StackExchange.Redis;

namespace amazon.APIs.Extentions
{
    public static class AppServicesExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {

            services.AddScoped<IpaymentService, PaymentService>();

            services.AddScoped<IOrderService ,OrderService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));      //instead of IGenaricRepository


            services.AddAutoMapper(typeof(MappingProfiles));


            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
            //services.AddSingleton<IConnectionMultiplexer>

            return services;

        }
    }
}
