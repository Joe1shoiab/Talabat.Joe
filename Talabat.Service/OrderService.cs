using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            var OrderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var ProductItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(ProductItem.Id, ProductItem.Name, ProductItem.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, item.Price, item.Quantity);
                    OrderItems.Add(orderItem);
                }
            }
            var Subtotal = OrderItems.Sum(item => item.Price * item.Quantity);

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var Order = new Order(buyerEmail, OrderItems, shippingAddress, deliveryMethod!, Subtotal);
            await _unitOfWork.Repository<Order>().AddAsync(Order);
            //TODO: Save to db
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) 
                return null;

            return Order;
        }

        public  Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return  _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
            var Order = _unitOfWork.Repository<Order>().GetByIdAsyncSpec(spec);
            return Order;
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            var Orders = _unitOfWork.Repository<Order>().GetAllAsyncSpec(spec);
            return Orders;
        }
    }
}