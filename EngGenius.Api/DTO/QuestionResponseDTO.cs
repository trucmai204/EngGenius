namespace EngGenius.Api.DTO
{
    public class QuestionResponseDTO
    {
        public string Content { get; set; }
        public List<string> Choices { get; set; }
        public int ResultChoiceIndex { get; set; }
        public string ExplainationForTheResult { get; set; }
    }
}
