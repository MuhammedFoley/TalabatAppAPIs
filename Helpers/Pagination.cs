using TalabatAppAPIs.Dtos;

namespace TalabatAppAPIs.Helpers
{
    public class Pagination<T>
    {


        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyCollection<T> Data { get; set; }

        public Pagination(int pageIndex, int pageSize,int count, IReadOnlyCollection<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count=count;
            Data = data;
        }
    }
}
