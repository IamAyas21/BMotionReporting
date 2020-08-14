using BMotionReporting.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BMotionReporting.App_Start
{
    public class CheckAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            //skip if AllowAnonymous
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)) return;

            try
            {
                if (String.IsNullOrEmpty(SessionManager.NIP()))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = filterContext.HttpContext.Request.FilePath }));
                }
                else
                {
                    //if (this.Roles != "")
                    //{
                    //    bool retval = false;

                    //    if (SessionManager.IsAdministrator())
                    //    {
                    //        retval = true;
                    //    }
                    //    else
                    //    {
                    //        int privilegeId = SessionManager.PrivilegeId();
                    //        var dtPrivilege = Common.ExecuteQuery(String.Format("dbo.PRIVILEGE_MENU_SELECT_SP {0},'{1}'", privilegeId, this.Roles));// _db.sp_PrivilegeMenuSelect(privilegeId, this.Roles).ToList();
                    //        if (dtPrivilege.Rows.Count > 0)
                    //        {
                    //            retval = Convert.ToBoolean(dtPrivilege.Rows[0]["Permission"].ToString());
                    //        }
                    //    }

                    //    if (!retval)
                    //    {

                    //        filterContext.Result = new RedirectToRouteResult(
                    //                                new RouteValueDictionary
                    //                                {
                    //                                    { "controller", "Home" },
                    //                                    { "action", "Error403" }
                    //                                });
                    //    }

                    //}
                }

            }
            catch (Exception)
            {
                filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary
                                                    {
                                                        { "controller", "Home" },
                                                        { "action", "Error403" }
                                                    });

            }
        }
    }
}