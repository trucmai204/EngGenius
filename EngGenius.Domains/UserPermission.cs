using EngGenius.Domains.Enum;

namespace EngGenius.Domains
{
    public class UserPermission
    {
        public EnumPermission Id { get; }
        public int? MaxQuestionPerDay { get; set; }
        public int? MaxTestPerDay { get; set; }
        public bool CanUseChatbot { get; set; }
    }

}
