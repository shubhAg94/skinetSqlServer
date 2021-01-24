using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.OrderAggregate
{
    //This is going to have an Id as a BaseEntity and is going to have its own table in our database.
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {

        }

        public OrderItem(ProductItemOrdered itemOrdered, decimal price, int quantity)
        {
            ItemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
        }

        //OrderItem class contains ProductItemOrdered that snapshots of the item that we were ordering.
        public ProductItemOrdered ItemOrdered { get; set; }

        //[Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
