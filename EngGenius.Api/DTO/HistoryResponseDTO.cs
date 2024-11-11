using EngGenius.Domains.Enum;

namespace EngGenius.Api.DTO
{
    public class HistoryResponseDTO
    {
        public EnumActionType ActionTypeId { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public DateTime ActionTime { get; set; }
    }
}
