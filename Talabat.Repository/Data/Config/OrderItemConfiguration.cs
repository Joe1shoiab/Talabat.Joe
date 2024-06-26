﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(i => i.ProductItemOrdered, io =>
            {
                io.WithOwner();
                io.Property(io => io.ProductItemId).HasColumnName("ProductItemId");
                io.Property(io => io.ProductName).HasColumnName("ProductName");
                io.Property(io => io.PictureUrl).HasColumnName("PictureUrl");
            });

            builder.Property(i => i.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
