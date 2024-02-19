using Amazon.Core.Entities;
using Amazon.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Repository.Data.Config
{
    public class OrderConfigrations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, Address => Address.WithOwner()); // el hagat el gowa el address

            builder.Property(O => O.Status)
                   .HasConversion(

                    OStatus => OStatus.ToString(),
                    OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)

                    );

            builder.Property(O => O.SubTotal)
                   .HasColumnType("decimal(18,2)");

            builder.HasMany(O => O.Items)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
