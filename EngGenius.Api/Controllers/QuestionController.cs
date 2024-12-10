using EngGenius.Api.DTO;
using EngGenius.Api.Helper;
using EngGenius.Database;
using EngGenius.Domains;
using EngGenius.Domains.Enum;
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
            var user = await _db.User.FindAsync(userId);

            StringBuilder prompt = new StringBuilder();

            prompt.AppendLine("You are an AI expert specializing in English language education.");
            prompt.AppendLine("Your task is to generate a set of high-quality, engaging, and educational questions designed to help users improve their English language skills.");
            prompt.AppendLine("Use the following inputs to customize the questions:");
            prompt.AppendLine($"1. Topic: {questionRequest.Topic.Trim()}");
            prompt.AppendLine($"2. Number of Questions: {questionRequest.TotalQuestion}");
            prompt.AppendLine($"3. Question Types: {string.Join(", ", questionRequest.QuestionTypes.Select(t => EnumHelper.GetEnumDescription(t)))}");
            prompt.AppendLine($"4. User Level: {EnumHelper.GetEnumDescription(user.LevelId)}");
            prompt.AppendLine();
            prompt.AppendLine("Question Requirements:");
            prompt.AppendLine("1. Create questions with clear and concise content, appropriate for the specified topic and user level.");
            prompt.AppendLine("2. Each question must include:");
            prompt.AppendLine("   - Content: The main text of the question, written in proper English.");
            prompt.AppendLine("   - Choices: A list of 3-5 options for answering the question. The options must be varied and meaningful.");
            prompt.AppendLine("   - ResultChoiceIndex: The index (starting from 0) of the correct answer in the list of choices.");
            prompt.AppendLine("   - ExplainationForTheResult: A detailed explanation of the correct answer in Vietnamese, ensuring it is simple and easy to understand.");
            prompt.AppendLine();
            prompt.AppendLine("3. Questions should align with the user level:");
            prompt.AppendLine("   - For Beginners, focus on foundational topics (e.g., common words, basic grammar).");
            prompt.AppendLine("   - For Intermediate, include moderate challenges like idioms, phrasal verbs, and sentence structuring.");
            prompt.AppendLine("   - For Advanced, focus on nuanced grammar, complex vocabulary, and critical reading.");
            prompt.AppendLine();
            prompt.AppendLine("4. Make sure the questions are diverse in type and designed to help users practice English in practical contexts.");
            prompt.AppendLine();
            prompt.AppendLine("Output Format:");
            prompt.AppendLine("The output must be in JSON format, structured as an array of objects using this class definition:");
            prompt.AppendLine();
            prompt.AppendLine("public class QuestionResponseDTO");
            prompt.AppendLine("{");
            prompt.AppendLine("    public string Content { get; set; } // Main question text");
            prompt.AppendLine("    public List<string> Choices { get; set; } // List of possible answers");
            prompt.AppendLine("    public int ResultChoiceIndex { get; set; } // Index of the correct choice");
            prompt.AppendLine("    public string ExplainationForTheResult { get; set; } // Detailed explanation in Vietnamese");
            prompt.AppendLine("}");
            prompt.AppendLine();
            prompt.AppendLine("Example Output for Topic: Vocabulary, Level: Beginner:");
            prompt.AppendLine();
            prompt.AppendLine("[");
            prompt.AppendLine("    {");
            prompt.AppendLine("        \"Content\": \"What is the opposite of 'hot'?\",");
            prompt.AppendLine("        \"Choices\": [\"Cold\", \"Warm\", \"Soft\", \"Dry\"],");
            prompt.AppendLine("        \"ResultChoiceIndex\": 0,");
            prompt.AppendLine("        \"ExplainationForTheResult\": \"'Cold' là từ trái nghĩa với 'hot', nghĩa là lạnh trong tiếng Anh.\"");
            prompt.AppendLine("    },");
            prompt.AppendLine("    {");
            prompt.AppendLine("        \"Content\": \"Which word means 'a place where books are kept'?\",");
            prompt.AppendLine("        \"Choices\": [\"Library\", \"School\", \"Office\", \"Kitchen\"],");
            prompt.AppendLine("        \"ResultChoiceIndex\": 0,");
            prompt.AppendLine("        \"ExplainationForTheResult\": \"'Library' là nơi chứa sách, còn các từ khác không liên quan đến sách.\"");
            prompt.AppendLine("    }");
            prompt.AppendLine("]");
            prompt.AppendLine();
            prompt.AppendLine("Now generate the JSON output based on the given inputs.");

            var generator = new GenAI.Generator
            {
                ApiKey = user.ApiKey,
            };
            var response = await generator.GenerateContent(prompt.ToString(), true);
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
