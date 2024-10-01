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
        private readonly AppDbContext _db;
        public UserController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _db.User
                .Include(u => u.Permission)
                .ToListAsync();
        }

        [HttpGet("Login")]
        public async Task<ActionResult<UserPermission>> Login(string email, string password)
        {
            var user = await _db.User
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
            var emailExisted = await _db.User
                .AnyAsync(u => u.Email == user.Email.ToLower().Trim());

            if (emailExisted)
            {
                return BadRequest("Email đã tồn tại!");
            }

            user.PermissionId = EnumPermission.Basic;

            _db.User.Add(user);
            await _db.SaveChangesAsync();
            return Ok(user);
        }
    }
}
