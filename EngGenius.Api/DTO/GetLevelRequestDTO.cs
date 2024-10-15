using EngGenius.Domains.Enum;

namespace EngGenius.Api.DTO
{
    public class GetLevelRequestDTO
    {
        public EnumLevel Id { get; set; }
        public string Description { get; set; }
    }
}
