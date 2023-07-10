using AutoMapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProjectManagementAPI.DTO;
using Repositories;
using Repositories.Impl;
using System.Security.Claims;
using System.Text.Json;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository = new UserRepository();
        private IMapper _mapper;
        public UsersController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [EnableQuery]
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers()
        {
            var users = _mapper.Map<List<UserDTO>>(_userRepository.GetUsers());
            return Ok(users.AsQueryable());
        }
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] LoginRequestDTO login)
        {
            var user = _mapper.Map<UserDTO>(_userRepository.GetByUsernameAndPassword(login.Username, login.Password));
            //if (user != null)
            //{ 
            //    // Tạo JWT
            //    var jwtService = new JwtService();
            //    var token = jwtService.GenerateToken(user);
            //    return Ok(token);
            //}

            return Ok(user);
        }
        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserDTO user)
        {
            var upuser = _mapper.Map<User>(user);
            _userRepository.UpdateUser(upuser);
            return Ok();
        }
        [HttpPost]
        public IActionResult CreateUser([FromBody] NewUserDTO user)
        {
            var newuser = _mapper.Map<User>(user);
            _userRepository.SaveUser(newuser);
            return Ok();
        }
    }
}
