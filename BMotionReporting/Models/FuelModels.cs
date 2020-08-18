using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Models
{
    public class FuelModels
    {
        public int FuilId { get; set; }
        public string Name { get; set; }
        public Nullable<double> Price { get; set; }
        public string IsSubsidy { get; set; }
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}