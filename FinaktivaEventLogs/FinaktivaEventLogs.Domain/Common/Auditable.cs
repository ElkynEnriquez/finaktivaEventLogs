namespace FinaktivaEventLogs.Domain.Common
{
    public class Auditable
    {
        public Guid Id { get; set; }
        public Guid CreateRegisterByUserId { get; set; }
        public DateTime CreateRegisterDate { get; set; }
        public Guid? UpdateRegisterByUserId { get; set; }
        public DateTime? UpdateRegisterDate { get; set; }
    }
}
