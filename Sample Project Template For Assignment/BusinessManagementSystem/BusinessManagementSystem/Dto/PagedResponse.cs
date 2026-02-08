namespace BusinessManagementSystem.Dto
{
    public class PagedResponse<T>:ResponseDto<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(List<T> datas, int pageNumber, int pageSize, int totalPages, int totalRecords)
        {
            Datas = datas;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalRecords = totalRecords;
        }
    }
}
