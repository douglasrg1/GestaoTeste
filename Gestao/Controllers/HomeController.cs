using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestao.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult About()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            

            return View();
        }

        public ActionResult Contact()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }
    }
}