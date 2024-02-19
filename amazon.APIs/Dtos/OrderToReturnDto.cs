﻿using Amazon.Core.Entities.Order_Aggregate;

namespace amazon.APIs.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 

        public string Status { get; set; } 

        public Address ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }

        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } 

        public decimal SubTotal { get; set; }


        public string PaymentIntentId { get; set; } = string.Empty; //untill i start with payment cycle

        public decimal Total {  get; set; }
    }
}
