namespace FinaktivaEventLogs.Services.DTO.Common
{
    public class AuditableDto
    {
        public Guid Id { get; set; }
        public Guid CreateRegisterByUserId { get; set; }
        public DateTime CreateRegisterDate { get; set; }
        public Guid? UpdateRegisterByUserId { get; set; }
        public DateTime? UpdateRegisterDate { get; set; }
    }
}
