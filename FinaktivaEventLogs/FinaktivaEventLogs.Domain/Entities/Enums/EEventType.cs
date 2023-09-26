using System.Runtime.Serialization;

namespace FinaktivaEventLogs.Domain.Entities.Enums
{
    public enum EEventType
    {
        [EnumMember(Value = "Api")] Api,
        [EnumMember(Value = "Formulario de eventos manuales")] Form,
    }
}
