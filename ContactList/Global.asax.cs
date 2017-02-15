using ContactList.App_Start;
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Web.Security;
using ContactList.Models;
using System.Web;
using System.Linq;
using ContactList.Filters;
using ContactList.DAL;

namespace ContactList
{
    public class Global : System.Web.HttpApplication
    {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogActionRequestAttribute());
            filters.Add(new GeneralExceptionHandlerAttribute());
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterGlobalFilters(GlobalFilters.Filters);

            //FilterConfig.Configure(GlobalFilters.Filters);

            RouteConfig.Configure(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            var context = System.Web.HttpContext.Current;
            context.Session["MakeSessionIDStaticForEntireSession"] = "";
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (DAL.ContactListContext entities = new DAL.ContactListContext())
                        {
                            User user = entities.Users.SingleOrDefault(u => u.username == username);

                            roles = user.roles;
                        }
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            //ContactListContext.LogException(ex);

            //RouteData routeData = new RouteData();
            //routeData.Values.Add("controller", "Errors");
            //routeData.Values.Add("action", "Error");

            //Response.Clear();
            //Server.ClearError();
            //Response.TrySkipIisCustomErrors = true;
            //Response.ContentType = "text/html";

            //IController errorController = new Controllers.ErrorController();
            //errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));


            var httpException = ex as HttpException;
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");


            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Page not found.
                        routeData.Values.Add("action", "NotFound");
                        break;
                    case 500:
                        // Server error.
                        routeData.Values.Add("action", "Error");
                        break;
                    default:
                        routeData.Values.Add("action", "Error");
                        break;
                }
            }

            routeData.Values.Add("error", ex);
            Server.ClearError();
            IController errorController = new Controllers.ErrorController();
            errorController.Execute(new RequestContext(
                new HttpContextWrapper(Context), routeData));

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}