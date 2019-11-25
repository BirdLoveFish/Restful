using System;
using System.Collections.Generic;
using System.Text;

namespace Restful.Core.Models
{
    public class PaginationBase
    {
        private int _pageSize = 10;
        public int PageIndex { get; set; } = 0;
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = value > _maxPageSize ? _maxPageSize : value;
        }
        public string OrderBy { get; set; } = "Id";
        private int _maxPageSize { get; set; } = 100;

        public string Fields { get; set; }
    }
}
