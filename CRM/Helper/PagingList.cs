using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.Helper
{
    public class PagingList<T> : List<T>
    {
        public long CurrentPage { get; set; }
        public long TotalPage { get; set; }
        public long PageSize { get; set; }
        public long TotalCount { get; set; }

        public PagingList(List<T>items, long totalcount, long pageNumber, long pageSize)
        {
            TotalCount = totalcount;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(totalcount / (double)pageSize);
            AddRange(items);
        }
        public static PagingList<T>CreateAsync(IQueryable<T> source, long pageNumber, long pageSize)
        {

            var totalCount = source.Count();
            var items = source.Skip(((int)pageNumber -1) * (int)pageSize).Take((int)pageSize).ToList();

            return new PagingList<T>(items, totalCount, pageNumber, pageSize);
        }

       
    }
}
