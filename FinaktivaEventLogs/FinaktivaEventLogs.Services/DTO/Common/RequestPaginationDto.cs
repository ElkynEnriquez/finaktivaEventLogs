namespace FinaktivaEventLogs.Services.DTO.Common
{
    public class RequestPaginationDto
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public FilterValueDto? FilterValue { get; set; }
    }
}
