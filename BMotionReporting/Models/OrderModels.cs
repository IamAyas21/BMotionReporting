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

    public class NewOrder
    {
        public string NIP { get; set; }
        public string Nama { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Quota { get; set; }
        public FuelOrder Fuel { get; set; }
    }

    public class FuelOrder
    {
        public List<FuelItemOrder> Subsidi { get; set; }
        public List<FuelItemOrder> NonSubsidi { get; set; }
    }

    public class FuelItemOrder
    {
        public string FuelId { get; set; }
        public string Fuel { get; set; }
    }

    public class OrderParameter
    {
        public string NIP { get; set; }
        public List<OrderParameterItem> Orders { get; set; }
    }

    public class OrderParameterItem
    {
        public string FuelId { get; set; }
        public string Liter { get; set; }
    }
}