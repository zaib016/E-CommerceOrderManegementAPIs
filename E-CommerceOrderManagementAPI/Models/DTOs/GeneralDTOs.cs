namespace E_CommerceOrderManagementAPI.Models.DTOs
{
    public class GeneralDTOs
    {
    }
    public class PaginatedListDTOs<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public static PaginatedListDTOs<T> FromIQueryable(IQueryable<T> source, PaginationAndSortingDTOs paginationAndSortingDTOs)
        {
            return new PaginatedListDTOs<T>()
            {
                Items = source
                .Skip((paginationAndSortingDTOs.PageIndex - 1) * paginationAndSortingDTOs.PageSize)
                .Take(paginationAndSortingDTOs.PageSize).ToList(),
                TotalCount = source.Count()
            };
        }
    }
    public class PaginationAndSortingDTOs
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int OffSet { get { return (PageIndex - 1) * PageSize; } }
 
    }
}
