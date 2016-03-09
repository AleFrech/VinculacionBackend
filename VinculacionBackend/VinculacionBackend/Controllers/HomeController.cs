using System.Web.Http.Cors;
using System.Web.Mvc;

namespace VinculacionBackend.Controllers
{
    public class HomeController : Controller
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
         

            return View();
        }
    }
}
