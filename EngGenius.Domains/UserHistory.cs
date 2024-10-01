using EngGenius.Domains.Enum;

namespace EngGenius.Domains
{
    public class UserHistory
    {
        public int Id { get; }
        public EnumActionType ActionTypeId { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public DateTime ActionTime { get; set; }
        public int UserId { get; set; }
        public bool IsSuccess { get; set; }

        public virtual ActionType ActionType { get; set; }
        public virtual User User { get; set; }
    }

}
