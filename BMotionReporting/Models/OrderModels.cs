using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Models
{
    public class OrderModels
    {
        public string OrderNo { get; set; }
        public string NIP { get; set; }
        public string isVerify { get; set; }
        public List<OrderDetailViewModels> OrderDetailList { get; set; }
    }
}