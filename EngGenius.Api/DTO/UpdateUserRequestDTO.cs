using EngGenius.Domains.Enum;

namespace EngGenius.Api.DTO
{
    public class UpdateUserRequestDTO
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? ApiKey { get; set; }
        public EnumLevel? LevelId { get; set; }
    }
}
