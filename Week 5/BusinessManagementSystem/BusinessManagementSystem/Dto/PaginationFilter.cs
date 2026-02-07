namespace BusinessManagementSystem.Dto
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string FieldSearch { get; set; }
        //public PaginationFilter(int pageNumber, int pageSize, string fieldSearch)
        //{
        //    PageNumber = pageNumber;
        //    PageSize = pageSize==-1? 1000000 : pageSize;
        //    FieldSearch = fieldSearch;
        //}
    }
}
