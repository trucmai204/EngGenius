﻿using EngGenius.Database;
using EngGenius.Domains;
using GenAI;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EngGenius.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DictionaryController(AppDbContext context)
        {
            _db = context;
        }

        [HttpGet("Search")]
        public async Task<ActionResult<string>> Search(int userId, string apiKey, string keyword, string? context)
        {
            var promptBuilder = new StringBuilder();

            var generator = new Generator
            {
                ApiKey = apiKey.Trim(),
            };

            keyword = keyword.Trim();

            promptBuilder.Append("Bạn là từ một điển Anh-Việt siêu ưu việt sử dụng công nghệ AI vào việc tra cứu. Nhiệm vụ của bạn là giúp tôi giải nghĩa tiếng Anh.");
            promptBuilder.AppendLine("Nếu từ/cụm từ được input là một thứ vô nghĩa hoặc không tồn tại trong tiếng Anh hoặc không thể giải nghĩa được hoặc quá tục tĩu, bạn hãy đưa ra output là 'Không thể giải nghĩa'");
            promptBuilder.Append($"Hãy cho tôi lời giải thích của '{keyword}'");
            promptBuilder.Append(!string.IsNullOrEmpty(context) ? $" trong ngữ cảnh '{context.Trim()}'. " : string.Empty);
            promptBuilder.AppendLine("Nội dung output của bạn phải bao gồm 11 phần:");
            promptBuilder.AppendLine($"- Tiêu đề: '{keyword.ToUpper()}'");
            promptBuilder.AppendLine($"- Phiên âm và từ loại của '{keyword}', nếu input không phải là từ vựng mà là thành ngữ thì không cần phiên âm");
            promptBuilder.AppendLine($"- Giải nghĩa của '{keyword}' trong ngữ cảnh được cung cấp (nếu có), nếu không có ngữ cảnh thì cung cấp tối đa 10 nghĩa phổ biến nhất của '{keyword}' kèm lời giải thích chi tiết");
            promptBuilder.AppendLine($"- Cung cấp tối thiểu 5 ví dụ về trường hợp sử dụng kèm của '{keyword}' và một số từ vựng khác có liên quan.");
            promptBuilder.AppendLine("- Cung cấp tối thiểu 3 từ đồng nghĩa và 3 từ trái nghĩa nếu có, đồng thời giải thích chi tiết về chúng.");
            promptBuilder.AppendLine($"- Cung cấp một số mẫu câu hoặc thành ngữ hoặc cụm từ phổ biến chứa '{keyword}'.");
            promptBuilder.AppendLine($"- Cung cấp thông tin về từ gốc và các từ phái sinh của '{keyword}' (nếu có)");
            promptBuilder.AppendLine($"- Cung cấp thông tin về lịch sử hình thành của từ '{keyword}' (nếu có).");
            promptBuilder.AppendLine($"- Các dạng biến đổi của '{keyword}' được tra cứu như thì quá khứ, thì hiện tại, dạng số nhiều, dạng so sánh,... (nếu có).");
            promptBuilder.AppendLine($"- Một số fun facts ít người biết liên quan đến '{keyword}' (nếu có).");
            promptBuilder.AppendLine("Cách trình bày output của bạn phải thật dễ hiểu và chi tiết, tuy nhiên không được quá dài dòng.");

            var result = await generator.Generate(promptBuilder.ToString());

            var userHistory = new UserHistory
            {
                Input = $"{keyword}\n{context}".Trim(),
                Output = result,
                ActionTime = DateTime.Now,
            };

            return Ok(result);
        }
    }
}
