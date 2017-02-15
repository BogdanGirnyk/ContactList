using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactList.DAL;

namespace ContactList.Filters
{
    public class GeneralExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                var msg = new string[2];

                msg[0] = filterContext.Exception.Message;

                ContactListContext.LogException(filterContext.Exception);

                HttpContext.Current.Session["CustomErrorMessage"] = msg[0];


                filterContext.Result = new ViewResult
                {
                    ViewName = "Exception",
                    ViewData = new ViewDataDictionary<string>(msg[0])
                };
                filterContext.ExceptionHandled = true;

            }
        }
    }
}