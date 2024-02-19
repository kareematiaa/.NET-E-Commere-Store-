using Amazon.Core;
using Amazon.Core.Entities;
using Amazon.Core.Entities.Order_Aggregate;
using Amazon.Core.Repositories;
using Amazon.Core.Services;
using Amazon.Core.Specifications.OrderSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IpaymentService _paymentService;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenericRepository<Order> _ordersRepo;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IpaymentService paymentService

           ///IGenericRepository<Product> productRepo,
            ///IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            ///IGenericRepository<Order> ordersRepo
            )
        {
            _basketRepository = basketRepository;  //didnt put this with unitofwork becuase this is call with diffrent database redis
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            ///_productRepo = productRepo;
            ///_deliveryMethodRepo = deliveryMethodRepo;
            ///_ordersRepo = ordersRepo;
        }
        public async Task<Order?> CreatOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get basket from basket repo

            var basket = await _basketRepository.GetBasketAsync(basketId);

            // 2. Get selected items at basket fropm products repo

            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0) 
            {
                foreach (var item in basket.Items)
                {

                    var productRepo = _unitOfWork.Repository<Product>();
                    if (productRepo is not null) 
                    { 
                        var product = await productRepo.GetByIdAsync(item.Id);

                        if(product is not null)
                        {
                              var productItemOrder = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                               var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

                               orderItems.Add(orderItem);

                        }
                    }

                }

            }

          

            // 3. Calculate SubTotal

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);



            // 4. Get delivery method from deliveryMethods repo

            DeliveryMethod deliveryMethod = new DeliveryMethod();

            var deliveryMethodsRepo = _unitOfWork.Repository<DeliveryMethod>();

            if(deliveryMethodsRepo is not null)
                   deliveryMethod= await deliveryMethodsRepo.GetByIdAsync(deliveryMethodId);


            // 5. Create order

            var spec = new OrderWithPaymentIntentIdSpec(basket.PaymentIntentId);

            var existingOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            if (existingOrder is not null) 
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);

                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal, basket.PaymentIntentId);

            var ordersRepo = _unitOfWork.Repository<Order>();

            if(ordersRepo is not null)
            {
                await ordersRepo.Add(order);

                // 6. Save to Database [TODO]

                var result = await _unitOfWork.Complete();

                if (result > 0)
                    return order;
            }

            return null;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveyMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return deliveyMethods;
        }

        public Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);
            var order = _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            if (order is null)
                return null;

            return order;


        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail); 

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return orders;
        }
    }
}
