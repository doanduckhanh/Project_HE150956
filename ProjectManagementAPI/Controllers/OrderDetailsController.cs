using AutoMapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using Repositories.Impl;
using Repositories;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private IOrderDetailRepository repository = new OrderDetailRepository();
        private readonly IMapper _mapper;

        public OrderDetailsController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult Post([FromBody] NewOrderDetailDTO order)
        {
            repository.SaveOrderDetails(_mapper.Map<OrderDetail>(order));
            return Ok(order);
        }
    }
}
