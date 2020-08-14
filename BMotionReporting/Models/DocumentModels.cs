using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Models
{
    public class DocumentModels
    {
        public Nullable<long> No { get; set; }
        public string DocumentNo { get; set; }
        public string NIP { get; set; }
        public string DocumentFile { get; set; }
        public Nullable<int> Quota { get; set; }
        public string ExpDate { get; set; }
        public string IsVerify { get; set; }
        public string PathDocument { get; set; }
        public HttpPostedFileBase PDF { get; set; }
        public string CreatedDate { get; set; }
    }
}