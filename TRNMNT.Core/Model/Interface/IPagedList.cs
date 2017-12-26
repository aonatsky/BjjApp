using System.Collections.Generic;

namespace TRNMNT.Core.Model.Interface
{
    public class IPagedList<T>
    {
        public List<T> InnerList { get; }
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}