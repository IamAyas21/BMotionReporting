using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Models
{
    public class StruckModels
    {
        public string OrderNo { get; set; }
        public string OutletNo { get; set; }
        public string OutletName { get; set; }
        public List<StruckOrderDetails> StruckOrderDetails { get; set; }
    }

    public class StruckOrderDetails
    {
        public string FuelName { get; set; }
        public Nullable<int> Liter { get; set; }
        public string CreatedDate { get; set; }
    }
}