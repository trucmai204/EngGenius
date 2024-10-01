using EngGenius.Domains.Enum;

namespace EngGenius.Domains
{
    public class User
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public EnumLevel LevelId { get; set; }
        public EnumPermission PermissionId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Level Level { get; set; }
        public virtual UserPermission Permission { get; set; }
    }
}
