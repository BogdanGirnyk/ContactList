using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContactList.DAL;
using ContactList.Models;

namespace ContactList.Controllers
{
    public class LoggingsController : Controller
    {
        private ContactListContext db = new ContactListContext();

        // GET: Loggings
        public ActionResult Index()
        {
            return View(db.Loggings.ToList().OrderByDescending(x => x.CreatedDate));
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
