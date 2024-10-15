using EngGenius.Api.DTO;
using EngGenius.Database;
using EngGenius.Domains.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EngGenius.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly AppDbContext _db;
        public QuestionController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("CreateQuestions")]
        public async Task<ActionResult<QuestionResponseDTO>> CreateQuestions(EnumLevel level, int userId, int totalQuestions)
        {
            return Ok();
        }
    }
}
