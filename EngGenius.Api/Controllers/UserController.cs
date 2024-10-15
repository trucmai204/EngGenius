using EngGenius.Api.DTO;
using EngGenius.Api.Helper;
using EngGenius.Database;
using EngGenius.Domains;
using EngGenius.Domains.Enum;
using GenAI;
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
        [ResponseCache(Duration = int.MaxValue, Location = ResponseCacheLocation.Any, NoStore = false)] 
        public async Task<ActionResult<LoginResponseDTO>> Login(string email, string password)
        {
            var user = await _db.User
                .Include(u => u.Permission)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return BadRequest("Email hoặc mật khẩu của bạn không đúng!");
            }

            var loginResponseDTO = new LoginResponseDTO
            {
                Name = user.Name,
                Level = new LoginResponseDTO.UserLevel
                {
                    Id = user.LevelId,
                    Name = EnumHelper.GetEnumDescription(user.LevelId)
                },
                Permission = new LoginResponseDTO.PermissionInfo
                {
                    Id = user.PermissionId,
                    Name = EnumHelper.GetEnumDescription(user.PermissionId)
                }
            };
            return Ok(loginResponseDTO);
        }

        [HttpGet("GetEnglishLevels")]
        //[ResponseCache(Duration = 1, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<ActionResult<GetLevelRequestDTO>> GetEnglishLevels()
        {
            var levels = await _db.Level
                .AsNoTracking()
                .Select(l => new GetLevelRequestDTO
                {
                    Id = l.Id,
                    Description = $"{l.Name} - {l.Description}",
                })
                .ToListAsync();

            return Ok(levels);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDTO user)
        {
            var emailExisted = await _db.User
                .AsNoTracking() // chi doc không the sua doi
                .AnyAsync(u => u.Email == user.Email.ToLower().Trim());

            if (emailExisted)
            {
                return BadRequest("Email đã tồn tại!");
            }

            var generator = new Generator
            {
                ApiKey = user.ApiKey
            };
            try
            {
                var content = await generator.GenerateContent("say hello");
            }
            catch 
            {
                return BadRequest("API Key không hợp lệ!");
            }
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                ApiKey = user.ApiKey,
                LevelId = user.LevelId,
                PermissionId = EnumPermission.Free,
                IsDeleted = false
            };

            _db.User.Add(newUser);
            await _db.SaveChangesAsync();

            return Created();
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(int userId, [FromBody] UpdateUserRequestDTO updateDTO)
        {
            var user = await _db.User.FindAsync(userId);
            if (user == null) 
            {
                return BadRequest($"Người dùng có Id = {userId} không tồn tại!");
            }

            user.Name = updateDTO.Name != null ? updateDTO.Name : user.Name;
            user.Password = updateDTO.Password != null ? updateDTO.Password : user.Password;
            user.LevelId = updateDTO.LevelId != null ? (EnumLevel)updateDTO.LevelId : user.LevelId;

            if (updateDTO.ApiKey != null) 
            {
                var generator = new Generator
                {
                    ApiKey = user.ApiKey
                };
                try
                {
                    var content = await generator.GenerateContent("say hello"); 
                }
                catch
                {
                    return BadRequest("API Key không hợp lệ!");
                }
                user.ApiKey = updateDTO.ApiKey;
            }
           
            _db.User.Update(user);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("History")]
        public async Task<ActionResult<List<HistoryResponseDTO>>> GetHistory(int userId, EnumActionType actionType)
        {
            var history = await _db.UserHistory
                .AsNoTracking()
                .Where(h => h.UserId == userId && h.ActionTypeId == actionType)
                .OrderByDescending(h => h.ActionTime)
                .Select(h => new HistoryResponseDTO
                {
                    Input = h.Input,
                    Output = h.Output,
                    ActionTime = h.ActionTime
                })
                .ToListAsync();
            return Ok(history);
        }
    }
}
