using EngGenius.Api.DTO;
using EngGenius.Database;
using EngGenius.Domains;
using GenAI;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
            string essayCriteria = @"
                1. Task Achievement:
                   - Understanding the prompt: Does the essay respond correctly and fully to the prompt? Does the writer address the main issue of the topic?
                   - Clear arguments: Does the essay stick to and develop the main ideas related to the topic? Are the arguments presented coherently and without deviating from the topic?
                   - Level of detail: Does the essay provide specific examples and arguments to support the main points? Avoid being too general or overly concise without details.

                2. Coherence and Cohesion:
                   - Logical arrangement: Are the main and supporting ideas presented in a logical order, easy to follow, and clearly divided between paragraphs?
                   - Paragraph transitions: Do the paragraphs link well through transitions or sentence structures that help the flow of ideas?
                   - Clear essay structure: The essay should have a clear introduction, body, and conclusion. The introduction should introduce the topic and main argument, while the conclusion should summarize the entire argument.

                3. Lexical Resource:
                   - Vocabulary variety: The writer should use a diverse range of vocabulary and not repeat too many basic words. This helps make the essay more engaging and demonstrates linguistic flexibility.
                   - Correct word usage: Words should be appropriate to the context and avoid errors in meaning. Pay attention to collocations and idioms.
                   - Avoid overly simple words: Instead of using simple words, the writer can try more academic or complex words to show a higher level of proficiency.

                4. Grammatical Range and Accuracy:
                   - Diverse grammatical structures: The writer needs to demonstrate the ability to use different sentence structures (simple, compound, complex, conditional sentences, passive voice, relative clauses, etc.).
                   - Grammatical accuracy: Avoid basic errors related to tenses, singular/plural forms, verb usage, prepositions, and verb conjugation. Errors in grammar should not affect the reader's understanding.
                   - Consistent tense usage: Avoid unreasonable tense shifts, especially when writing about past or future events.

                5. Spelling and Punctuation:
                   - Correct spelling: Avoid spelling mistakes, especially with common words or easily confused terms.
                   - Proper punctuation: Use punctuation correctly to separate sentences and ideas, making the essay easier to follow and maintaining a proper rhythm. Avoid mistakes like missing periods, improper commas, or overly long sentences without punctuation.

                6. Tone and Style:
                   - Appropriate tone: The essay should have a tone that suits the prompt requirements (formal, semi-formal, informal). Avoid using words or phrases that are inappropriate for the context.
                   - Clear style: The essay’s expression should be clear and straightforward, without causing confusion. Avoid overly complex phrases that make the essay cumbersome.

                Example of applying these criteria:
                Assume the task is to write about 'The advantages and disadvantages of online learning'.

                - Task Achievement: The essay must fully present both the advantages and disadvantages of online learning without missing any part.
                - Coherence and Cohesion: The introduction introduces the development of online learning, followed by the body dividing into paragraphs about the pros and cons. Use transitions like 'On the one hand,' 'On the other hand,' to link ideas between paragraphs.
                - Lexical Resource: Use words like 'flexibility,' 'cost-effective,' 'lack of face-to-face interaction' to express ideas accurately.
                - Grammatical Range and Accuracy: Use complex sentences like 'Although online learning offers flexibility, it also presents challenges in maintaining student engagement.'
                - Spelling and Punctuation: Avoid mistakes like 'learining' instead of 'learning' and use punctuation correctly to break long sentences when needed.
                - Tone and Style: The tone should be formal, appropriate for the educational context, avoiding overly casual language.";

            Console.WriteLine(essayCriteria);

            stringBuilder.AppendLine("You are an English teacher with over 30 years of experiences. This is criteria for you to review my writting: ");
            stringBuilder.AppendLine(essayCriteria.Trim());
            stringBuilder.AppendLine($"Topic is: '{reviewWriting.Topic.Trim()}'. \n This is my writting: ");
            stringBuilder.AppendLine(reviewWriting.Content.Trim());
            stringBuilder.AppendLine("Your review must be in Vietnamese and easy to understand for Vietnamese, and be structured following each criteria as described above..");

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
