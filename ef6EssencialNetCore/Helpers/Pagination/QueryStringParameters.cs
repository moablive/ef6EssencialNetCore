
namespace ef6EssencialNetCore.Helpers.Pagination;

    public class QueryStringParameters
    {
        // 50 intens no Maximo
        const int maxPageSize = 50;
        public int PageNumber { get; set; }
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
