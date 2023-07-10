using AutoMapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.DTO;
using Repositories.Impl;
using Repositories;
using Microsoft.AspNetCore.OData.Query;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private IFoodRepository repository = new FoodRepository();
        private readonly IMapper _mapper;
        public FoodsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/<CategoriesController>
        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<FoodDTO>> Get()
        {
            var foods = _mapper.Map<List<FoodDTO>>(repository.GetFoods());

            return Ok(foods.AsQueryable());
        }


        // POST api/<CategoriesController>
        [HttpPost]
        public IActionResult Post([FromBody] FoodDTO food)
        {
            repository.SaveFood(_mapper.Map<Food>(food));
            return Ok(food);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] FoodDTO food)
        {
            repository.UpdateFood(_mapper.Map<Food>(food));
            return Ok(food);
        }
    }
}
