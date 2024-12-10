using EngGenius.Domains.Enum;

namespace EngGenius.Api.DTO
{
    public class LoginResponseDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public UserLevel Level { get; set; }
        public PermissionInfo Permission { get; set; }

        public class UserLevel
        {
            public EnumLevel Id { get; set; }
            public string Name { get; set; }
        }

        public class PermissionInfo
        {
            public EnumPermission Id { get; set; }
            public string Name { get; set; }
        }

    }
}
