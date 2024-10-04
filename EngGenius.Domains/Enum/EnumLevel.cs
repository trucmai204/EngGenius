using System.ComponentModel;

namespace EngGenius.Domains.Enum
{
    public enum EnumLevel
    {
        [Description("Beginner (Người mới bắt đầu)")]
        A1 = 1,

        [Description("Elementary (Sơ cấp)")]
        A2 = 2,

        [Description("Intermediate (Trung cấp)")]
        B1 = 3,

        [Description("Upper-Intermediate (Trung cao cấp)")]
        B2 = 4,

        [Description("Advanced (Cao cấp)")]
        C1 = 5,

        [Description("Proficient (Thành thạo)")]
        C2 = 6
    }
}
