using EngGenius.Database;
using EngGenius.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EngGenius.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllUser")]
        public async Task<ActionResult<List<Person>>> GetAllUsers()
        {
            return await _context.Person.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetUserById(int id)
        {
            return await _context.Person.FindAsync(id);
        }
    }
}
