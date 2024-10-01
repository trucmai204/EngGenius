using EngGenius.Domains.Enum;

namespace EngGenius.Domains
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public EnumLevel LevelId { get; set; }
        public EnumPermission PermissionId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual UserPermission? Permission { get; set; }
    }
}
