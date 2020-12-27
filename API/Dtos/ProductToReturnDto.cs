using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    //We should never expose our Domain Models to the outside world. We should Only expose Dto's.
    //Here we segregated ProductType, ProductBrand from their objects.
    //If we send Product model only to client, we will send ProductType, ProductBrand objects to client and clients
    //needs to do hardwork of segregation according to their need.
    //So as a best API practises we sholud return the data what actually client needs.
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
    }
}
