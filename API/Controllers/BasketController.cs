using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket)
        {
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}

/*
 Don't forget we're not storing anything in our actual database here. it's just a place where customers can leave 
 their baskets behind in our memory. So if they come back they can pick up where they left off. And what we will store 
 on the client side is, the basketId and we'll use that from the client to go and get whatever basket left inside the memory.

 And if they leave a basket in memory or read this for a month and they don't come back to it then it's gonna be destroyed anyway.
 And if that was the case they just have to go and put the items back in a new virtual basket.

 it's in memory and you're probably thinking well what happens if the server restarts. Will I not lose all of the baskets 
 that are stored in memory. Well Redis is going to take a snapshot of the database every minute or so and when redis starts up
 again it's gonna load the snapshots that it saved on disk into memory again. So it's not a big deal really.

 And the baskets are not going to be something that we store permanently on our server anyway. So if a basket gets 
 destroyed or is lost for whatever reason then the worst case scenario is that the customer simply has an empty basket 
 again and I'll have to put items back inside it. It's not the same as losing an order that would be different.
 */
