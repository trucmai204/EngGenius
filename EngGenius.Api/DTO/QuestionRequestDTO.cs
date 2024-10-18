using EngGenius.Domains.Enum;

namespace EngGenius.Api.DTO
{
    public class QuestionRequestDTO
    {
        public string Topic { get; set; }
        public int TotalQuestion { get; set; }
        public List<EnumQuestionType> QuestionTypes { get; set; }
    }
}
