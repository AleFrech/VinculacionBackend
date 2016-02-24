using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VinculacionBackend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            if (MailManager.CheckDomainValidity("carlos.varela@gmail.edu"))
            {
                MailManager.SendSimpleMessage("carlos.varela@gmail.edu", "holaaa", "probando");
            }

            return View();
        }
    }
}
