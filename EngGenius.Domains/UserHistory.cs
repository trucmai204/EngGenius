using EngGenius.Domains.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngGenius.Domains
{
    public class UserHistory
    {
        public int Id { get; set; }
        public EnumActionType ActionTypeId { get; set; }
        public string Input { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Output { get; set; }
        public DateTime ActionTime { get; set; }
        public int UserId { get; set; }
        public bool IsSuccess { get; set; }

        public virtual User User { get; set; }
    }
}
