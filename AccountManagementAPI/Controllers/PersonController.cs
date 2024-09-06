using AccountManagementAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AccountManagementAPI.Repository;
using Microsoft.AspNetCore.Authorization;

namespace AccountManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepo _personRepo;
        public PersonController(AccountDBContext dbContext, IPersonRepo personrepo)
        {
            _personRepo = personrepo;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_personRepo.GetPeople());
        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_personRepo.GetPersonById(id));
        }

    }
}
