using AutoMapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using ProjectManagementAPI.DTO;
using Repositories;
using Repositories.Impl;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrderRepository repository = new OrderRepository();
        private readonly IMapper _mapper;

        public OrdersController(IMapper mapper){
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult Post([FromBody] NewOrderDTO order)
        {
            repository.SaveOrder(_mapper.Map<Order>(order));
            return Ok(order);
        }
    }
}
