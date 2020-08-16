using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Models
{
    public class UserModels
    {
        public string NIP { get; set; }
        public string Name { get; set; }
        public string Profesi { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Telp { get; set; }
        public string IsVerify { get; set; }
        public int RoleId { get; set; }
        public string OutletNo { get; set; }
        public int Quota { get; set; }
        public string City { get; set; }
        public string ExpiredDate { get; set; }
        public HttpPostedFileBase KTP { get; set; }
        public HttpPostedFileBase PDF { get; set; }
        public PagedList<DocumentModels> DocumentList { get; set; }
    }
}