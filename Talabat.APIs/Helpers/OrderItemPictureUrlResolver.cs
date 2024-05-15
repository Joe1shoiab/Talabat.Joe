using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public OrderItemPictureUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProductItemOrdered.PictureUrl))
                return $"{_config["ApiUrl"]}{source.ProductItemOrdered.PictureUrl}";

            return string.Empty;
            
        }
    }
}
