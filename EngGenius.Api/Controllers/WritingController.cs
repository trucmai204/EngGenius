using EngGenius.Api.DTO;
using EngGenius.Database;
using EngGenius.Domains;
using GenAI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace EngGenius.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WritingController : ControllerBase
    {
        private readonly AppDbContext _db;
        public WritingController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("Review")]
        public async Task<ActionResult<string>> Review(int userId, [FromBody] ReviewWritingRequestDTO reviewWriting)
        {
            var stringBuilder = new StringBuilder();
            var user = await _db.User
                .FindAsync(userId);

            var generator = new Generator
            {
                ApiKey = user.ApiKey,
            };
            stringBuilder.Append($"Thầy ơi, thầy nhận xét giúp em bài viết Tiếng Anh này. Yêu cầu Topic của nó là: {reviewWriting.Topic.Trim()}");
            stringBuilder.AppendLine("Bài viết của em là: ");
            stringBuilder.AppendLine(reviewWriting.Content.Trim());

            var result = await generator.GenerateContent(stringBuilder.ToString());

            var userHistory = new UserHistory
            {
                ActionTypeId = Domains.Enum.EnumActionType.Writting,
                Input = $"{reviewWriting.Topic.Trim()}\n{reviewWriting.Content.Trim()}".Trim(),
                Output = result,
                ActionTime = DateTime.Now,
                UserId = userId,
                IsSuccess = true
            };
            _db.UserHistory.Add(userHistory);
            await _db.SaveChangesAsync();

            return Ok(result);
        }
    }
}
