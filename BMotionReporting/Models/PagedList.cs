using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Models
{
    public class PagedList<T>
    {
        public List<T> Content { get; set; }
        public Int32 CurrentPage { get; set; }
        public Int32 PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int TotalPages
        {
            get { return (TotalRecords == 0 || PageSize == 0) ? 0 : (int)Math.Ceiling((decimal)TotalRecords / PageSize); }
        }
    }
}