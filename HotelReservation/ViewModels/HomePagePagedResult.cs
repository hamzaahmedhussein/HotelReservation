namespace HotelReservation.ViewModels
{
    public class HomePagePagedResult<T>
    {
        public List<T> Results { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public FilteredPaginatedRooms Filter { get; set; }

        public HomePagePagedResult(List<T> results, int count, int pageNumber, int pageSize, FilteredPaginatedRooms filter)
        {
            Results = results;
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Filter = filter;
        }
    }
}
