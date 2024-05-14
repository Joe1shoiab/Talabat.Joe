using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShipToAddress, s => s.WithOwner());

            builder.Property(s => s.Status)
                .HasConversion(
                           o => o.ToString(),
                           o => (OrderStatus)Enum.Parse(typeof(OrderStatus),
                           o));

            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(builder => builder.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");

        }
    }
}