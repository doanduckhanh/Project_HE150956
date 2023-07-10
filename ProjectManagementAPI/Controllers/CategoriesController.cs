using AutoMapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProjectManagementAPI.DTO;
using Repositories;
using Repositories.Impl;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryRepository repository = new CategoryRepository();
        private readonly IMapper _mapper;
        public CategoriesController(IMapper mapper){
            _mapper = mapper;
            }

        // GET: api/<CategoriesController>
        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> Get()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(repository.GetCategories());
            return Ok(categories.AsQueryable());
        }


        // POST api/<CategoriesController>
        [HttpPost]
        public IActionResult Post([FromBody] CategoryDTO category)
        {
            repository.SaveCategory(_mapper.Map<Category>(category));
            return Ok(category);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CategoryDTO category)
        {
            repository.UpdateCategory(_mapper.Map<Category>(category));
            return Ok(category);
        }
    }
}
