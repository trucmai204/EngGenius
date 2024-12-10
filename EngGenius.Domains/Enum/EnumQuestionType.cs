using System.ComponentModel;

namespace EngGenius.Domains.Enum
{
    public enum EnumQuestionType
    {
        [Description("Most Suitable Word (Chọn từ thích hợp nhất)")]
        MostSuitableWord = 1,

        [Description("Verb Conjugation (Chia động từ)")]
        VerbConjugation = 2,

        [Description("Conditional Sentences (Câu điều kiện)")]
        ConditionalSentences = 3,

        [Description("Indirect Speech (Câu gián tiếp)")]
        IndirectSpeech = 4,

        [Description("Sentence Completion (Điền vào chỗ trống)")]
        SentenceCompletion = 5,

        [Description("Grammar (Ngữ pháp)")]
        Grammar = 6,

        [Description("Collocation (Phối hợp từ)")]
        Collocation = 7,

        [Description("Synonym/Antonym (Từ đồng nghĩa/trái nghĩa)")]
        SynonymAntonym = 8,

        [Description("Vocabulary (Từ vựng)")]
        Vocabulary = 9,

        [Description("Error Identification (Xác định lỗi sai)")]
        ErrorIdentification = 10,

        [Description("Word Formation (Chuyển đổi từ loại)")]
        WordFormation = 11
    }

}
