using System.Runtime.Serialization;

namespace FinaktivaEventLogs.Services.DTO.Enums
{
    public enum EEventTypeDto
    {
        [EnumMember(Value = "Api")] Api,
        [EnumMember(Value = "Formulario de eventos manuales")] Form,
    }
}
