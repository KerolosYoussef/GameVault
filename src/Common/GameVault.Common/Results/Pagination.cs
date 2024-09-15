using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameVault.Common.Results
{
    public class Pagination<T>
    {
        public Pagination(int pageIndex, int pageSize, int totalItems, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = totalItems;
            Data = data;
        }
        public Pagination()
        {
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
