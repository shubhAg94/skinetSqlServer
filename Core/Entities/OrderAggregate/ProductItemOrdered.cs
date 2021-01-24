using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.OrderAggregate
{
    /*This one allows us to take a snapshot, if you like, of the product item that was ordered.
     And this is just based on the fact that product names could change the picture could change but we always want 
     to keep the order as the order was made rather than relating it to another entity. And this allows us to do that.
     */
    public class ProductItemOrdered
    {
        //Because we're dealing with entity framework we also need a parameter lists constructor here as well.
        public ProductItemOrdered()
        {

        }

        //This is not going to have an Id column because it's going to be owned by the order itself.
        public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductItemId { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }
    }
}
