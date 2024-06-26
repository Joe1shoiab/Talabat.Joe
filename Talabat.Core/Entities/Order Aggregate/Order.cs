﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(string buyerEmail, ICollection<OrderItem> orderItems, Address shipToAddress, DeliveryMethod deliveryMethod, decimal subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            OrderItems = orderItems;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;


        }

        public string BuyerEmail { get; }
        public DateTimeOffset OrderDate { get; } = DateTimeOffset.Now;
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; } // for stripe payment intent id
        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Cost;
        }
    }
}
