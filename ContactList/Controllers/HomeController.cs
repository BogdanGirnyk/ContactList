using System.Web.Mvc;
using ContactList.DAL;
using System.Linq;


namespace ContactList.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {


           // return RedirectToAction("Index", "Contacts");
            return View();
        }
    }
}