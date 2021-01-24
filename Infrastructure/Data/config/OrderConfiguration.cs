using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        /*What we want to do in here is tell entity framework about our address.*/
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //Configures the relationship where the target entity is owned by (or part of) this entity.
            builder.OwnsOne(o => o.ShipToAddress, a => 
            {
                a.WithOwner();
            });
            //Here what we're doing is we want to get enum to a string rivaling the integer that's gonna be returned by default.
            builder.Property(s => s.Status)
                .HasConversion(
                    o => o.ToString(),
                    o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
                );

            /*So if we do delete an order we also delete any order items that are part of this particular order and
            the order is going to have a one to many relationship with the order items.*/
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
