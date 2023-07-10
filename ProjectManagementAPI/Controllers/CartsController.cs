using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Impl;
using Repositories;
using ProjectManagementAPI.DTO;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private ICartRepository repository = new CartRepository();
        private readonly IMapper _mapper;
        public CartsController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [HttpGet("AddToCart/{foodid}/{ordercartid}")]
        public IActionResult AddToCart(int foodid, string ordercartid)
        {
            repository.AddToCart(foodid, ordercartid);
            return Ok();
        }
        [HttpGet("IncreaseFromCart/{cartid}/{recordid}")]
        public IActionResult IncreaseFromCart(string cartid, int recordid)
        {
            repository.IncreaseFromCart(cartid, recordid);
            return Ok();
        }
        [HttpGet("RemoveCart/{cartid}/{recordid}")]
        public IActionResult RemoveCart(string cartid, int recordid)
        {
            repository.RemoveCart(cartid, recordid);
            return Ok();
        }
        [HttpGet("GetCart/{cartid}")]
        public ActionResult<IEnumerable<CartDTO>> GetCarts(string cartid)
        {
            var carts = _mapper.Map<List<CartDTO>>(repository.GetCart(cartid));
            return Ok(carts);
        }
        [HttpGet("GetCount/{cartid}")]
        public IActionResult GetCount(string cartid)
        {
            var count = repository.GetCount(cartid);
            return Ok(count);
        }
        [HttpGet("GetTotal/{cartid}/{code}")]
        public IActionResult GetTotal(string cartid, string code)
        {
            var total = repository.GetTotal(cartid, code);
            return Ok(total);
        }
        [HttpGet("RemoveFromCart/{cartid}/{recordid}")]
        public IActionResult RemoveFromCart(string cartid, int recordid)
        {
            repository.RemoveFromCart(cartid, recordid);
            return Ok();
        }
    }
}
