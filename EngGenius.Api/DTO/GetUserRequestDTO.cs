using EngGenius.Domains.Enum;

namespace EngGenius.Api.DTO
{
    public class GetUserRequestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public EnumLevel LevelId { get; set; }

        public EnumPermission PermissionId { get; set; }
    }
}
