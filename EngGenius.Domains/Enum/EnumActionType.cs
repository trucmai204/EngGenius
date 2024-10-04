using System.ComponentModel;

namespace EngGenius.Domains.Enum
{
    public enum EnumActionType
    {
        [Description("Tra cứu từ điển")]
        SearchWord = 1,

        [Description("Luyện viết")]
        Writting = 2,

        [Description("Làm bài tập")]
        DoTest = 3
    }
}
