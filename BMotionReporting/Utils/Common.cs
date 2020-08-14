using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Utils
{
    public class Common
    {
        [NonAction]
        public static SelectList ToSelectList(DataTable table, string valueField, string textField, string selectedValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text", selectedValue);
        }
    }
}