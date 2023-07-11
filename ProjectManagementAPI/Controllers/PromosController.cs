using AutoMapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProjectManagementAPI.DTO;
using Repositories.Impl;
using Repositories;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromosController : ControllerBase
    {
        private IPromoRepository repository = new PromoRepository();
        private readonly IMapper _mapper;
        public PromosController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/<CategoriesController>
        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<PromoDTO>> Get()
        {
            var promos = _mapper.Map<List<PromoDTO>>(repository.GetPromos());

            return Ok(promos.AsQueryable());
        }


        // POST api/<CategoriesController>
        [HttpPost]
        public IActionResult Post([FromBody] PromoDTO promo)
        {
            repository.SavePromo(_mapper.Map<Promo>(promo));
            return Ok(promo);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PromoDTO promo)
        {
            repository.SavePromo(_mapper.Map<Promo>(promo));
            return Ok(promo);
        }
    }
}
