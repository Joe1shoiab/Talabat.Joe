using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }

        public string Status { get; set; }
        public decimal Total { get; set; }

        public string PaymentIntendId { get; set; }


    }
}
