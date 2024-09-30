using EngGenius.Database;
using EngGenius.Domains;
using EngGenius.Domains.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EngGenius.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllUser")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _context.User
                .Include(u => u.Level)
                .Include(u => u.Permission)
                .ToListAsync();
        }

        [HttpGet("Login")]
        public async Task<ActionResult<UserPermission>> Login(string email, string password)
        {
            var user = await _context.User
                .Include(u => u.Permission)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return BadRequest("Email hoặc mật khẩu của bạn không đúng!");
            }

            return Ok(user.Permission);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            var isExist = await _context.User.AnyAsync(u => u.Email == user.Email.ToLower().Trim());
            if (isExist)
            {
                return BadRequest("Email đã tồn tại!");
            }

            user.PermissionId = EnumPermission.Basic;

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
