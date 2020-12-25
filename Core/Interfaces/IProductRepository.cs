using System.Threading.Tasks;
using System.Collections.Generic;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);

        /* List is a very generic that supports lots of different things we can add items to it we can remove items from it.
        But with IReadOnlyList We can only read as the name implies from what we're sending back and because we are returning
        a list we don't need the functionality that comes with a normal list that allows us to add and remove items from it.
        So when we're returning something like this we can be more specific about what we're returning.*/
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductsBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductsTypesAsync();
    }
}