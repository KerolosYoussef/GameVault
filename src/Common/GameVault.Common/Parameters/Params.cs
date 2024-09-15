using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameVault.Common.Parameters
{
    public abstract class Params
    {
        private const int MaxPageSize = 500;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? OrderBy { get; set; }
        public string? OrderByType { get; set; } = "ASC";
    }
}
