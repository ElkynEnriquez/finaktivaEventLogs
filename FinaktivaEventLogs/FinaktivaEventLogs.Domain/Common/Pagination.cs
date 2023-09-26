namespace FinaktivaEventLogs.Domain.Common
{
    public class Pagination<TEntity>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public FilterValue FilterValue { get; set; }
        public int PagesQuantity { get; set; }
        public List<TEntity> Data { get; set; }
        public int TotalRows { get; set; }
    }
}
