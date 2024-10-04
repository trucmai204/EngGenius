using EngGenius.Domains.Enum;

namespace EngGenius.Domains
{
    public class UserPermission
    {
        public EnumPermission Id { get; set; }
        public string Name { get; set; }
        public int? MaxWrittingPerDay { get; set; }
        public int? MaxTestPerDay { get; set; }
        public bool CanUseChatbot { get; set; }
    }

}
