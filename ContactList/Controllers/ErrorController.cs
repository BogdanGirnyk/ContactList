using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ContactList.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
    
            Exception ex = RouteData.Values["error"] as Exception;
            return View("Exception", (object)("404 NOT FOUND"+Environment.NewLine+ex.Message));
        }

        public ActionResult Error()
        {

            Exception ex = RouteData.Values["error"] as Exception;
            return View("Exception", (object)ex.Message);
        }
    }
}
