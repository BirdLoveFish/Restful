using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Core.Models
{
    public class PagintedList<T> : List<T> where T : class
    {
        public PaginationBase PaginationBase { get; }
        public int TotalItemsCount { get; private set; }
        public int PageCount
        {
            get => (TotalItemsCount / PaginationBase.PageSize)
                + (TotalItemsCount % PaginationBase.PageSize > 0 ? 1 : 0);
        }
        public bool HasPrevious => PaginationBase.PageIndex > 0;
        public bool HasNext => PaginationBase.PageIndex < (PageCount - 1);

        public PagintedList(int pageSize, int pageIndex, int totalItemsCount, IEnumerable<T>data)
        {
            PaginationBase = new PaginationBase
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            TotalItemsCount = totalItemsCount;
            AddRange(data);
        }
    }
}
