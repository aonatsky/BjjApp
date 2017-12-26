using System.Collections.Generic;
using TRNMNT.Core.Model.Interface;

namespace TRNMNT.Core.Model
{
    public class PagedList<T> : IPagedList<T>
    {
        public List<T> InnerList { get; }

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int total)
        {
            TotalCount = total;
            TotalPages = total / pageSize;
            if (total % pageSize > 0)
                TotalPages++;
            PageSize = pageSize;
            PageIndex = pageIndex;
            InnerList = new List<T>(source);
        }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageIndex > 0;
        public bool HasNextPage => PageIndex + 1 < TotalPages;
    }
}
