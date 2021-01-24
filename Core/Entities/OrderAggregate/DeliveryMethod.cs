using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.OrderAggregate
{
    /*This is going to allow the user to choose what sort of delivery they want.
     And this has a BaseEntity because we want it to have an Id. And we want our client to be able to retrieve
    the list of DeliveryMethod so that means there's going to be a table for delivery methods in our database
     */

    public class DeliveryMethod : BaseEntity
    {
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }

        //[Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
