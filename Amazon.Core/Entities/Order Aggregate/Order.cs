﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Entities.Order_Aggregate
{
    public class Order :BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate {  get; set; } = DateTimeOffset.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShippingAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; } //Navigational Property [ONE]

        public ICollection<OrderItem> Items { get; set;} = new HashSet<OrderItem>(); ////Navigational Property [MANY]

        public decimal SubTotal { get; set; }

        //[NotMapped]
        //public decimal Total  => SubTotal + DeliveryMethod.Cost;   //Derived Attribute 
        public decimal GetTotal()
         => SubTotal+ DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; } = string.Empty; //untill i start with payment cycle
    }
}
