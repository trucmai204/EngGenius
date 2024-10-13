using EngGenius.Domains.Enum;
using System.Reflection.Metadata;

namespace EngGenius.Api.DTO
{
    public class RegisterRequestDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public EnumLevel LevelId { get; set; }
    }
}
