using EngGenius.Api.DTO;
using EngGenius.Api.Helper;
using EngGenius.Database;
using EngGenius.Domains;
using EngGenius.Domains.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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

        [HttpGet("GetListQuestionTypes")]
        public ActionResult<List<EnumQuestionType>> GetListQuestionTypes()
        {
            var questionTypes = Enum.GetValues(typeof(EnumQuestionType))
                                    .Cast<EnumQuestionType>()
                                    .Select(q => new
                                    {
                                        Id = (int)q,
                                        Name = q.ToString(),
                                        Description = EnumHelper.GetEnumDescription(q)
                                    })
                                    .ToList();

            return Ok(questionTypes);
        }


        [HttpPost("CreateQuestions")]
        public async Task<ActionResult<List<QuestionResponseDTO>>> CreateQuestions(int userId, [FromBody] QuestionRequestDTO questionRequest)
        {
            var stringBuilder = new StringBuilder();
            var user = await _db.User.FindAsync(userId);

            stringBuilder.AppendLine($"Create a set of {questionRequest.TotalQuestion} English questions based on the topic of '{questionRequest.Topic.Trim()}' with types of question including {string.Join(',', questionRequest.QuestionTypes)}.");
            stringBuilder.AppendLine($"The questions should be appropriate for learners at an {user.LevelId.ToString()} - level English proficiency ({EnumHelper.GetEnumDescription(user.LevelId)}).") ;
            stringBuilder.AppendLine("Ensure the difficulty level matches the A-level, with questions focused on basic vocabulary, grammar, and simple sentence structures.") ;
            stringBuilder.AppendLine("Output must be a json array following c# class as below:");
            stringBuilder.AppendLine("public class QuestionResponseDTO\r\n    {\r\n        public string Content {  get; set; }\r\n       " +
                    " public List<string> Choices { get; set; }\r\n        public int ResultChoiceIndex { get; set; }\r\n       " +
                    " public string ExplainationForTheResult { get; set; } // must be in Vietnamese and explain the most detail\r\n    }");


            var generator = new GenAI.Generator
            {
                ApiKey = user.ApiKey,
            };
            var response = await generator.GenerateContent(stringBuilder.ToString(), true);
            var questions = JsonConvert.DeserializeObject<List<QuestionResponseDTO>>(response);

            var history = new UserHistory
            {
                ActionTypeId = EnumActionType.DoTest,
                Input = $"{questionRequest.Topic} - {questionRequest.TotalQuestion}",
                Output = JsonConvert.SerializeObject(questions),
                ActionTime = DateTime.Now,
                UserId = userId,
                IsSuccess = true
            };
            _db.UserHistory.Add(history);
            await _db.SaveChangesAsync();

            return Ok(questions);
        }
    }
}
